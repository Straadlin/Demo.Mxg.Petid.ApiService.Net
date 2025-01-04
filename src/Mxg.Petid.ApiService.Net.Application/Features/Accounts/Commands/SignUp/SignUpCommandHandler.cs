using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Mxg.Petid.ApiService.Net.Application.Common.Constants;
using Mxg.Petid.ApiService.Net.Application.Common.Exceptions;
using Mxg.Petid.ApiService.Net.Application.Common.Interfaces.Persistance;
using Mxg.Petid.ApiService.Net.Application.Common.Interfaces.Security;
using Mxg.Petid.ApiService.Net.Application.Common.Models.Security;
using Mxg.Petid.ApiService.Net.Application.Features.Accounts.Commands.SignUp.Dtos;
using Mxg.Petid.ApiService.Net.Domain.Entities;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Mxg.Petid.ApiService.Net.Application.Features.Accounts.Commands.SignUp;

/// <summary>
/// Command to sign up a new user.
/// </summary>
public class SignUpCommandHandler : IRequestHandler<SignUpCommand, SignUpDto>
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;
    private readonly ILogger<SignUpCommandHandler> logger;
    private readonly IEncryptSevice encryptSevice;

    public SignUpCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<SignUpCommandHandler> logger, IEncryptSevice encryptSevice)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
        this.logger = logger;
        this.encryptSevice = encryptSevice;
    }

    public async Task<SignUpDto> Handle(SignUpCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var existingUser = await FindUserAsync(null, request.Email, request.PhoneNumber);

            if (existingUser is not null)
                throw new ConflictException($"Username, Email or PhoneNumber has been used by another account.");

            var existingRole = await GetRoleAsync(RoleUserTypeConstants.TYPE_CUSTOMER_USER);

            if (existingRole is null)
                throw new BadRequestException($"Role doesn't exist.");

            City? existingCity = null;

            if (request.CityId is not null)
            {
                existingCity = await FindCityAsync(request);

                if (existingCity is null)
                    throw new BadRequestException($"City doesn't exist.");
            }

            Domain.Entities.Type? existingGenderType = null;

            if (request.GenderTypeId is not null)
            {
                existingGenderType = await FindCodeTypeByIdAsync(
                request.GenderTypeId, CollectionTypeConstants.GENDER_PERSON_TYPE_GNDPRS);

                if (existingGenderType is null)
                    throw new BadRequestException($"Gender type doesn't exist.");
            }

            var existingStatusAccountType = await FindCodeTypeByCodeAsync(
                StatusAccountTypeConstants.STATUS_ACCOUNT_TYPE_ACTIVE, CollectionTypeConstants.STATUS_ACCOUNT_TYPE_STSACC);

            if (existingStatusAccountType is null)
                throw new BadRequestException($"Status account type doesn't exist.");

            var existingAlgorithmPasswordType = await FindCodeTypeByCodeAsync(
                AlgorithmTypeConstants.ALGORITHM_PASSWORD_TYPE_SHA256_IDENTITY, CollectionTypeConstants.ALGORITHM_TYPE_ALGTCRYP);

            if (existingAlgorithmPasswordType is null)
                throw new BadRequestException($"Algorithm type doesn't exist.");

            var privateInfoEncrypted = EncryptPrivateInfo(request, existingCity, existingGenderType);

            var hashPasswordGenerated = GenerateHashPassword(request);

            var userCreated = await CreateUserAsync(
                request, existingAlgorithmPasswordType, hashPasswordGenerated, existingStatusAccountType,
                existingRole, existingCity, existingGenderType, privateInfoEncrypted);

            if (userCreated is null)
                throw new BadRequestException($"User could not be created.");

            return new SignUpDto
            {
                Username = userCreated.Username,
                Email = userCreated.Email,
                PhoneNumber = userCreated.PhoneNumber,
                FirstName = request.FirstName,
                LastName = request.LastName,
                GenderTypeId = existingGenderType?.Id,
                GenderTypeCode = existingGenderType?.Code,
                CityId = existingCity?.Id,
            };
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    private string EncryptPrivateInfo(SignUpCommand request, City? city, Domain.Entities.Type? genderType)
    {
        var info = new
        {
            request.FirstName,
            request.LastName,
            request.Birthdate,
            CityId = city?.Id,
            GenderTypeId = genderType?.Id,
        };

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = true
        };

        var serializedInfo = JsonSerializer.Serialize(info, options);

        var encryptedInfo = Encrypt(serializedInfo, request.Password);

        return encryptedInfo;
    }

    // TODO - DEBE ESTAR EN UN SERVICIO
    private static string Encrypt(string plainText, string key)
    {
        using (Aes aesAlg = Aes.Create())
        {
            // Convertir la llave a bytes (debe ser de 16, 24 o 32 bytes para AES)
            aesAlg.Key = Encoding.UTF8.GetBytes(key.PadRight(32).Substring(0, 32));
            aesAlg.IV = aesAlg.Key.Take(16).ToArray(); // Tomar los primeros 16 bytes como IV para simplificar

            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(plainText);
                    }
                    return Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
        }
    }

    private static string Decrypt(string cipherText, string key)
    {
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = Encoding.UTF8.GetBytes(key.PadRight(32).Substring(0, 32));
            aesAlg.IV = aesAlg.Key.Take(16).ToArray();

            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText)))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        return srDecrypt.ReadToEnd();
                    }
                }
            }
        }
    }

    private HashPasswordResult GenerateHashPassword(SignUpCommand request)//Este podría ir en un servicio.
    {
        var parameters = new HashPasswordParameters
        {
            Password = request.Password
        };

        var passwordHash = this.encryptSevice.GenerateHashPassword(parameters);

        return passwordHash;
    }

    private async Task<User?> FindUserAsync(string username = null, string email = null, string phoneNumber = null, string identificationCode = null)
    {
        try
        {
            var user = (await this.unitOfWork.Repository<User>()
                .GetAsync(
                    predicate: u =>
                        (
                            (!string.IsNullOrEmpty(username) && u.Username == username) || 
                            (!string.IsNullOrEmpty(email) && u.Email == email) || 
                            (!string.IsNullOrEmpty(phoneNumber) && u.PhoneNumber == phoneNumber)
                        ) &&
                        u.IsActive,
                    orderBy: null, includeString: null, disableTracking: true))
                .SingleOrDefault();

            return user;
        }
        catch (Exception ex)
        {
            // TODO - Falta grabar en los logs las excepciones y relanzar la excepción.
            throw;
        }
    }

    private async Task<Role?> GetRoleAsync(string roleType)
    {
        try
        {
            var role = (await this.unitOfWork.Repository<Role>().GetAsync(
                    predicate: r =>
                        r.Code == roleType &&
                        r.IsActive
                    )).SingleOrDefault();

            return role;
        }
        catch (Exception ex)
        {
            // TODO - Falta grabar en los logs las excepciones y relanzar la excepción.
            throw;
        }
    }

    private async Task<City?> FindCityAsync(SignUpCommand request)
    {
        try
        {
            var city = (await this.unitOfWork.Repository<City>()
                    .GetAsync(predicate: p =>
                        p.Id == request.CityId
                    , orderBy: null, includeString: null, disableTracking: true)).SingleOrDefault();

            return city;
        }
        catch (Exception ex)
        {
            // TODO - Falta grabar en los logs las excepciones y relanzar la excepción.
            throw;
        }
    }

    private async Task<Domain.Entities.Type?> FindCodeTypeByCodeAsync(string code, string collectionType)
    {
        try
        {
            var includes = new List<Expression<Func<Domain.Entities.Type, object>>>
            {
                t => t.CollectionType
            };

            var type = (await this.unitOfWork.Repository<Domain.Entities.Type>()
                .GetAsync(predicate: t =>
                    t.Code == code &&
                    t.IsActive &&
                    t.CollectionType.Code == collectionType &&
                    t.CollectionType.IsActive
                , orderBy: null, includes: includes, disableTracking: true))
                .SingleOrDefault();

            return type;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    private async Task<Domain.Entities.Type?> FindCodeTypeByIdAsync(Guid? idCodeType, string collectionType)
    {
        try
        {
            var includes = new List<Expression<Func<Domain.Entities.Type, object>>>
            {
                t => t.CollectionType
            };

            var type = (await this.unitOfWork.Repository<Domain.Entities.Type>()
            .GetAsync(predicate: t =>
                    t.Id == idCodeType &&
                    t.IsActive &&
                    t.CollectionType.Code == collectionType &&
                    t.CollectionType.IsActive
                , orderBy: null, includes: includes, disableTracking: true))
                .SingleOrDefault();

            return type;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    private async Task<User?> CreateUserAsync(
        SignUpCommand request, Domain.Entities.Type algorithmPasswordType, HashPasswordResult hashPasswordGenerated,
        Domain.Entities.Type statusAccountType, Role role, City? city, Domain.Entities.Type? genderType,
        string privateInfoEncrypted)
    {
        try
        {
            var userToAdd = new User
            {
                Username = request.Username!,
                Email = request.Email,
                EmailConfirmed = true,
                EmailConfirmedCode = null,
                PhoneNumber = request.PhoneNumber,
                PhoneNumberConfirmed = false,
                PhoneNumberConfirmedCode = null,
                Birthdate = request.Birthdate,
                PasswordHash = hashPasswordGenerated.Hash,
                PrivateInfoJson = privateInfoEncrypted,
                IsInfoEncrypted = true,
                GenderTypeId = genderType?.Id,
                AlgorithmPasswordTypeId = algorithmPasswordType.Id,
                CityId = city?.Id,
                EmployeedCompanyId = null,
                StatusAccountTypeId = statusAccountType.Id,
                RoleId = role.Id,
                CreatedBy = "-5"
            };

            this.unitOfWork.Repository<User>().AddEntity(userToAdd);
            var result = await this.unitOfWork.CompleteAsync();

            if (result < 1)
                return null;

            return userToAdd;
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}
﻿using BCrypt.Net;
using Regira.Security.Abstractions;
using Regira.Security.Core;
using Regira.Utilities;
using System.Text;

namespace Regira.Security.Hashing.BCryptNet;

public class Hasher : IHasher
{
    // https://github.com/BcryptNet/bcrypt.net
    private readonly string _salt;
    private readonly Encoding _encoding;
    private readonly string _algorithm;
    public Hasher(CryptoOptions? options = null)
    {
        _salt = options?.Secret ?? DefaultSecuritySettings.SaltKey;
        _encoding = options?.Encoding ?? DefaultSecuritySettings.Encoding;
        _algorithm = options?.AlgorithmType ?? DefaultSecuritySettings.HashAlgorithm;
    }

    public string Hash(string plainText)
    {
        var hashType = GetHashType(_algorithm);
        var hashed = BCrypt.Net.BCrypt.EnhancedHashPassword(plainText + _salt, hashType);
        return hashed.Base64Encode(_encoding);
    }

    public bool Verify(string plainText, string hashedValue)
    {
        var base64Hashed = hashedValue.Base64Decode(_encoding);
        var hashType = GetHashType(_algorithm);
        return BCrypt.Net.BCrypt.EnhancedVerify(plainText + _salt, base64Hashed, hashType);
    }

    private HashType GetHashType(string algorithm)
    {
        if (Enum.TryParse<HashType>(algorithm, true, out var hashType))
        {
            return hashType;
        }

        return HashType.SHA384;
    }
}
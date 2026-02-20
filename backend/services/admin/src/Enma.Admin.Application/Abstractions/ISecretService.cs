using Enma.Admin.Application.Dto.Other;

namespace Enma.Admin.Application.Abstractions;

public interface ISecretService
{
    KeyMaterial Generate(string tokenPrefix = "", int bytesCount = 32);
    string ComputeHash(string plainKey);
}
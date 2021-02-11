using System;

namespace JobsityChat.Core.Contracts
{
    public interface ITokenService
    {
        string GetNewToken(string authenticatedUserName);
    }
}

// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Abstractions.Errors;
using ExtDotNet.OAuth20.Server.Abstractions.Errors.Exceptions;
using ExtDotNet.OAuth20.Server.Abstractions.Errors.Exceptions.Authorize;
using ExtDotNet.OAuth20.Server.Abstractions.Errors.Exceptions.Common;
using ExtDotNet.OAuth20.Server.Abstractions.Errors.Exceptions.Token;
using ExtDotNet.OAuth20.Server.Options;
using Microsoft.Extensions.Options;

namespace ExtDotNet.OAuth20.Server.Default.Errors;

public class DefaultErrorResultProvider : IErrorResultProvider
{
    private readonly IErrorMetadataCollection _errorMetadataCollection;
    private readonly IOptions<OAuth20ServerOptions> _options;

    public DefaultErrorResultProvider(IErrorMetadataCollection errorMetadataCollection, IOptions<OAuth20ServerOptions> options)
    {
        _errorMetadataCollection = errorMetadataCollection;
        _options = options;
    }

    public IResult GetErrorResult(OAuth20Exception exception)
    {
        if (TryGetErrorResult(exception, out IErrorResult? result))
        {
            return result!;
        }
        else
        {
            throw new InvalidOperationException($"{nameof(exception)}:{exception}");
        }
    }

    public bool TryGetErrorResult(OAuth20Exception exception, out IErrorResult? result)
    {
        result = null;

        bool success = exception switch
        {
            AuthorizeException authorizeException => TryGetAuthorizeErrorResult(authorizeException, out result, exception.State, exception.Message),
            TokenException tokenException => TryGetTokenErrorResult(tokenException, out result, exception.State, exception.Message),
            CommonException commonException => TryGetCommonErrorResult(commonException, out result, exception.State, exception.Message),
            _ => false
        };

        return success;
    }

    public IErrorResult GetCommonErrorResult<TException>(TException exception, string? state, string? additionalInfo = null)
        where TException : CommonException
    {
        if (TryGetCommonErrorResult(exception, out IErrorResult? result, state, additionalInfo))
        {
            return result!;
        }
        else
        {
            throw new InvalidOperationException($"{nameof(exception)}:{exception}");
        }
    }

    public IErrorResult GetCommonErrorResult(DefaultCommonErrorType defaultErrorType, string? state = null, string? additionalInfo = null)
        => GetCommonErrorResult(defaultErrorType.GetFieldNameAttributeValue(_options.Value), state, additionalInfo);

    public IErrorResult GetCommonErrorResult(string commonErrorCode, string? state = null, string? additionalInfo = null)
    {
        if (TryGetCommonErrorResult(commonErrorCode, out IErrorResult? result, state, additionalInfo))
        {
            return result!;
        }
        else
        {
            throw new InvalidOperationException($"{nameof(commonErrorCode)}:{commonErrorCode}");
        }
    }

    public bool TryGetCommonErrorResult<TException>(TException exception, out IErrorResult? result, string? state, string? additionalInfo = null)
        where TException : CommonException
    {
        result = null;

        bool success = exception switch
        {
            Abstractions.Errors.Exceptions.Common.InvalidRequestException => TryGetCommonErrorResult(DefaultCommonErrorType.InvalidRequest, out result, state, additionalInfo),
            Abstractions.Errors.Exceptions.Common.CommonErrorException => TryGetCommonErrorResult(DefaultCommonErrorType.CommonError, out result, state, additionalInfo),
            Abstractions.Errors.Exceptions.Common.ServerConfigurationErrorException => TryGetCommonErrorResult(DefaultCommonErrorType.ServerConfigurationError, out result, state, additionalInfo),
            Abstractions.Errors.Exceptions.Common.InvalidScopeException => TryGetCommonErrorResult(DefaultCommonErrorType.InvalidScope, out result, state, additionalInfo),
            _ => false
        };

        return success;
    }

    public bool TryGetCommonErrorResult(DefaultCommonErrorType defaultErrorType, out IErrorResult? result, string? state = null, string? additionalInfo = null)
        => TryGetCommonErrorResult(defaultErrorType.GetFieldNameAttributeValue(_options.Value), out result, state, additionalInfo);

    public bool TryGetCommonErrorResult(string commonErrorCode, out IErrorResult? result, string? state = null, string? additionalInfo = null)
    {
        ErrorMetadata? errorMetadata = _errorMetadataCollection.CommonErrors.Values.FirstOrDefault(x => x.Code == commonErrorCode);

        if (errorMetadata is not null)
        {
            string? errorDescription = additionalInfo is null ?
                errorMetadata.Description :
                $"{errorMetadata.Description}({additionalInfo})";

            result = ErrorResult.Create(
                error: errorMetadata.Code,
                errorDescription: errorDescription,
                errorUri: errorMetadata.Uri,
                state
                );

            return true;
        }
        else
        {
            result = null;
            return false;
        }
    }

    public IErrorResult GetAuthorizeErrorResult<TException>(TException exception, string? state = null, string? additionalInfo = null)
        where TException : AuthorizeException
    {
        if (TryGetAuthorizeErrorResult(exception, out IErrorResult? result, state, additionalInfo))
        {
            return result!;
        }
        else
        {
            throw new InvalidOperationException($"{nameof(exception)}:{exception}");
        }
    }

    public IErrorResult GetAuthorizeErrorResult(DefaultAuthorizeErrorType defaultErrorType, string? state = null, string? additionalInfo = null)
        => GetAuthorizeErrorResult(defaultErrorType.GetFieldNameAttributeValue(_options.Value), state, additionalInfo);

    public IErrorResult GetAuthorizeErrorResult(string authorizeErrorCode, string? state = null, string? additionalInfo = null)
    {
        if (TryGetAuthorizeErrorResult(authorizeErrorCode, out IErrorResult? result, state, additionalInfo))
        {
            return result!;
        }
        else
        {
            throw new InvalidOperationException($"{nameof(authorizeErrorCode)}:{authorizeErrorCode}");
        }
    }

    public bool TryGetAuthorizeErrorResult<TException>(TException exception, out IErrorResult? result, string? state = null, string? additionalInfo = null)
        where TException : AuthorizeException
    {
        result = null;

        bool success = exception switch
        {
            Abstractions.Errors.Exceptions.Authorize.InvalidRequestException => TryGetAuthorizeErrorResult(DefaultAuthorizeErrorType.InvalidRequest, out result, state, additionalInfo),
            Abstractions.Errors.Exceptions.Authorize.UnauthorizedClientException => TryGetAuthorizeErrorResult(DefaultAuthorizeErrorType.UnauthorizedClient, out result, state, additionalInfo),
            Abstractions.Errors.Exceptions.Authorize.AccessDeniedException => TryGetAuthorizeErrorResult(DefaultAuthorizeErrorType.AccessDenied, out result, state, additionalInfo),
            Abstractions.Errors.Exceptions.Authorize.UnsupportedResponseTypeException => TryGetAuthorizeErrorResult(DefaultAuthorizeErrorType.UnsupportedResponseType, out result, state, additionalInfo),
            Abstractions.Errors.Exceptions.Authorize.InvalidScopeException => TryGetAuthorizeErrorResult(DefaultAuthorizeErrorType.InvalidScope, out result, state, additionalInfo),
            Abstractions.Errors.Exceptions.Authorize.ServerErrorException => TryGetAuthorizeErrorResult(DefaultAuthorizeErrorType.ServerError, out result, state, additionalInfo),
            Abstractions.Errors.Exceptions.Authorize.TemporarilyUnavailableException => TryGetAuthorizeErrorResult(DefaultAuthorizeErrorType.TemporarilyUnavailable, out result, state, additionalInfo),
            _ => false
        };

        return success;
    }

    public bool TryGetAuthorizeErrorResult(DefaultAuthorizeErrorType defaultErrorType, out IErrorResult? result, string? state = null, string? additionalInfo = null)
        => TryGetAuthorizeErrorResult(defaultErrorType.GetFieldNameAttributeValue(_options.Value), out result, state, additionalInfo);

    public bool TryGetAuthorizeErrorResult(string authorizeErrorCode, out IErrorResult? result, string? state = null, string? additionalInfo = null)
    {
        ErrorMetadata? errorMetadata = _errorMetadataCollection.AuthorizeErrors.Values.FirstOrDefault(x => x.Code == authorizeErrorCode);

        if (errorMetadata is not null)
        {
            string? errorDescription = additionalInfo is null ?
                errorMetadata.Description :
                $"{errorMetadata.Description}({additionalInfo})";

            result = ErrorResult.Create(
                error: errorMetadata.Code,
                errorDescription: errorDescription,
                errorUri: errorMetadata.Uri,
                state
                );

            return true;
        }
        else
        {
            result = null;
            return false;
        }
    }

    public IErrorResult GetTokenErrorResult<TException>(TException exception, string? state = null, string? additionalInfo = null)
        where TException : TokenException
    {
        if (TryGetTokenErrorResult(exception, out IErrorResult? result, state, additionalInfo))
        {
            return result!;
        }
        else
        {
            throw new InvalidOperationException($"{nameof(exception)}:{exception}");
        }
    }

    public IErrorResult GetTokenErrorResult(DefaultTokenErrorType defaultErrorType, string? state = null, string? additionalInfo = null)
        => GetTokenErrorResult(defaultErrorType.GetFieldNameAttributeValue(_options.Value), state, additionalInfo);

    public IErrorResult GetTokenErrorResult(string tokenErrorCode, string? state = null, string? additionalInfo = null)
    {
        if (TryGetTokenErrorResult(tokenErrorCode, out IErrorResult? result, state, additionalInfo))
        {
            return result!;
        }
        else
        {
            throw new InvalidOperationException($"{nameof(tokenErrorCode)}:{tokenErrorCode}");
        }
    }

    public bool TryGetTokenErrorResult<TException>(TException exception, out IErrorResult? result, string? state = null, string? additionalInfo = null)
        where TException : TokenException
    {
        result = null;

        bool success = exception switch
        {
            Abstractions.Errors.Exceptions.Token.InvalidRequestException => TryGetTokenErrorResult(DefaultTokenErrorType.InvalidRequest, out result, state, additionalInfo),
            Abstractions.Errors.Exceptions.Token.InvalidClientException => TryGetTokenErrorResult(DefaultTokenErrorType.InvalidClient, out result, state, additionalInfo),
            Abstractions.Errors.Exceptions.Token.InvalidGrantException => TryGetTokenErrorResult(DefaultTokenErrorType.InvalidGrant, out result, state, additionalInfo),
            Abstractions.Errors.Exceptions.Token.UnauthorizedClientException => TryGetTokenErrorResult(DefaultTokenErrorType.UnauthorizedClient, out result, state, additionalInfo),
            Abstractions.Errors.Exceptions.Token.UnsupportedGrantTypeException => TryGetTokenErrorResult(DefaultTokenErrorType.UnsupportedGrantType, out result, state, additionalInfo),
            Abstractions.Errors.Exceptions.Token.InvalidScopeException => TryGetTokenErrorResult(DefaultTokenErrorType.InvalidScope, out result, state, additionalInfo),
            _ => false
        };

        return success;
    }

    public bool TryGetTokenErrorResult(DefaultTokenErrorType defaultErrorType, out IErrorResult? result, string? state = null, string? additionalInfo = null)
        => TryGetTokenErrorResult(defaultErrorType.GetFieldNameAttributeValue(_options.Value), out result, state, additionalInfo);

    public bool TryGetTokenErrorResult(string tokenErrorCode, out IErrorResult? result, string? state = null, string? additionalInfo = null)
    {
        ErrorMetadata? errorMetadata = _errorMetadataCollection.TokenErrors.Values.FirstOrDefault(x => x.Code == tokenErrorCode);

        if (errorMetadata is not null)
        {
            string? errorDescription = additionalInfo is null ?
                errorMetadata.Description :
                $"{errorMetadata.Description}({additionalInfo})";

            result = ErrorResult.Create(
                error: errorMetadata.Code,
                errorDescription: errorDescription,
                errorUri: errorMetadata.Uri,
                state
                );

            return true;
        }
        else
        {
            result = null;
            return false;
        }
    }
}

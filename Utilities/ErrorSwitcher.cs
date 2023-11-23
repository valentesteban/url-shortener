using Microsoft.AspNetCore.Mvc;

namespace url_shortener.Utilities;

public class ErrorSwitcher : ControllerBase
{
    public (string, string) GetErrorFromException(string e)
    {
        var splits = e.Split("-");

        if (splits.Length != 2)
        {
            return (null, null);
        }

        var code = splits[0];
        var message = splits[1];

        return (code, message);
    }

    public ActionResult getResultFromError(string code, string message)
    {
        switch (code)
        {
            case "NT":
                return NotFound(message);
            case "BD" or "FB" or "IE":
                return BadRequest(message);
            case "UA":
                return Unauthorized(message);
            default:
                return BadRequest(message);
        }
    }
}
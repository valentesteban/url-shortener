using System.Collections;
using Microsoft.AspNetCore.Mvc;

namespace url_shortener.Models;

public class APIException : ControllerBase
{
    public enum Type
    {
        NOT_FOUND,
        BAD_REQUEST,
        UNAUTHORIZED,
        FORBIDDEN,
        INTERNAL_SERVER_ERROR
    }

    public enum Code
    {
        CT_01,
        CT_02,
        CT_03,
        
        URL_01,
        URL_02,
        URL_03,
        URL_04,
        
        DB_01,
        DB_02,
    }
    public static Exception CreateException(Code code, String message, Type type)
    {
        Exception e = new Exception();
        e.Data.Add("code", code.ToString());
        e.Data.Add("error", message);
        e.Data.Add("type", type.ToString());
        return e;
    }

    public ActionResult getResultFromError(Type type, IDictionary data)
    {
        switch (type)
        {
            case Type.NOT_FOUND:
                return NotFound(data);
            case Type.BAD_REQUEST:
                return BadRequest(data);
            case Type.UNAUTHORIZED:
                return Unauthorized(data);
            case Type.FORBIDDEN:
                return BadRequest(data);
            case Type.INTERNAL_SERVER_ERROR:
                return BadRequest(data);
            default:
                return BadRequest(data);
        }   
    }
}
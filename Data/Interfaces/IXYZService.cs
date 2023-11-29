using url_shortener.Entities;
using url_shortener.Models;

namespace url_shortener.Data.Interfaces;

public interface IXYZService
{
    public List<XYZ> GetAll();
    public XYZ? getUrlLongByShort(string urlShort);
    public XYZ? getById(int id);
    public XYZ? createUrl(XYZForCreationDTO creationDto);
    public bool isUrlShortExist(string urlShort);
    public bool isUrlLongExist(string urlLong);
    public void deleteUrl(int id);
    public void deleteUrl(string urlShort);
    public void addClick(int id);

}
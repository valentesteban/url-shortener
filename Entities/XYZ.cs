using SQLiteNetExtensions.Attributes;

namespace url_shortener.Entities;

public class XYZ
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string UrlShort { get; set; }
    public string UrlLong { get; set; }
    public int Clicks { get; set; }
    public int CategoryId { get; set; }
    [ForeignKey(typeof(User))]
    public int UserId { get; set;}
}
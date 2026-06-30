using System.ComponentModel.DataAnnotations.Schema;
using WebApplication7.Data;
using NetTopologySuite.Geometries;

namespace WebApplication7.Model
{
    [Table("bilgiler")]
    public class bilgi
    {
        public int Id { get; set; }

        [Column("konum")]
        public Point Konum { get; set; } = null!;

        [Column("sehir")]
        public string Sehir { get; set; } = string.Empty;
    }
}
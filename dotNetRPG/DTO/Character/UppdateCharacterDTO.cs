using dotNetRPG.Models;

namespace dotNetRPG.DTO.Character
{
    public class UppdateCharacterDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int HitPoints { get; set; }
        public int Strength { get; set; }
        public int Defence { get; set; }
        public int Intelligence { get; set; }
        public RpgClass Class { get; set; }

    }
}

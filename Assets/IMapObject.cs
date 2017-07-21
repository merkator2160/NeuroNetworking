using Assets.Models;

namespace Assets
{
    public interface IMapObject
    {
        Dimensions Dimensions { get; set; }
        Position Position { get; set; }

        void CalculatePosition();
    }
}
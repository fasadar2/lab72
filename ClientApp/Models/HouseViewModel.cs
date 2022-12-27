using LabsDB.Entity;

namespace ClientApp.Models;

public class HouseViewModel
{
    public HouseViewModel()
    {
        Houses = new List<House>();
    }

    public HouseViewModel(IEnumerable<House> houses)
    {
        Houses = houses;
    }

    public IEnumerable<House> Houses { get; init; }
}
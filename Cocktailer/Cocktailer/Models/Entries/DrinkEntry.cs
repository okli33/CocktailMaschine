namespace Cocktailer.Models.Entries
{
    public class DrinkEntry : IEntry
    {
        public string Brand { get; set; }
        public string Name { get; set; }
        public double Percentage { get; set; }

        public override string ToString()
        {
            return $"{Brand}/{Name}, {Percentage}%"; 
        }
        public override bool Equals(object obj)
        {
            if (obj is DrinkEntry)
            {
                var drobj = (DrinkEntry)obj;
                return drobj.Brand == this.Brand
                    && drobj.Name == this.Name
                    && drobj.Percentage == this.Percentage;
            }
            return false;
        }
    }
}

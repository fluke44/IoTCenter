namespace IoTCenter.Domain.ReturnTypes
{
    public class TempAndHum
    {
        public TempAndHum(decimal temp, decimal hum)
        {
            Temp = temp;
            Hum = hum;
        }

        public decimal Temp { get; }

        public decimal Hum { get; }
    }
}

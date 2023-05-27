namespace Scripts.Models
{
    public class ScoreModel 
    {
        public int Value { get; private set; }
        public bool ValueChangedFlag { get; private set; } = true;

        public void ResetFlag()
        {
            ValueChangedFlag = false;
        }

        public void AddValue(int amount)
        {
            Value += amount;
            ValueChangedFlag = true;
        }
    }
}
var banks = BatteryBankParser.Parse("inputs/day3.txt");
var sum = 0;
foreach (var bank in banks)
{
    var maxJoltage = bank.GetMaxJoltage(2);
    Console.WriteLine($"The largest joltage possible is {string.Join("", maxJoltage)}");
    sum += maxJoltage[0] * 10 + maxJoltage[1];
}

Console.WriteLine($"Total output joltage: {sum}");


public static class BatteryBankParser
{
    public static BatteryBank[] Parse(string path)
    {
        var lines = File.ReadAllLines(path);
        BatteryBank[] banks = new BatteryBank[lines.Length];
        for (int i = 0; i < lines.Length; i++)
        {
            var digits = lines[i].AsSpan();
            int[] batteries = new int[digits.Length];
            for (int j = 0; j < digits.Length; j++)
            {
                batteries[j] = int.Parse(digits[j].ToString());
            }
            banks[i] = new BatteryBank(batteries);
        }
        return banks;
    }
}

public record BatteryBank(int[] Batteries)
{
    public int[] GetMaxJoltage(int totalBatteries)
    {
        int batteryJolt1 = Batteries[0];
        int index1 = 0;
        int batteryJolt2 = Batteries[1];
        int index2 = 1;
        int maxJoltage = 10;
        for (int i = 0; i < Batteries.Length - 1; i++)
        {
            if (Batteries[i] * 10 + batteryJolt2 > maxJoltage)
            {
                batteryJolt1 = Batteries[i];
                index1 = i;
            }

            for (int j = i + 1; j < Batteries.Length; j++)
            {
                if (index2 <= index1 || batteryJolt1 * 10 + Batteries[j] > maxJoltage)
                {
                    batteryJolt2 = Batteries[j];
                    index2 = j;
                    maxJoltage = batteryJolt1 * 10 + batteryJolt2;
                }
            }
        }
        return [batteryJolt1, batteryJolt2];
    }
}
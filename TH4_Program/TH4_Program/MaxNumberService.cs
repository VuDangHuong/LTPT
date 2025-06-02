using System;
using System.Linq;

namespace MaxNumberService
{
    public class MaxNumberService : MarshalByRefObject, IMaxNumberService
    {
        public int TimSoLonNhat(int[] numbers)
        {
            if (numbers == null || numbers.Length == 0)
                throw new ArgumentException("Mảng không được rỗng");
            
            string[] numberStrings = numbers.Select(n => n.ToString()).ToArray();
            Console.WriteLine("Server nhan mang: " + string.Join(", ", numberStrings));
            int max = numbers.Max();
            Console.WriteLine($"Số lon nhat tim duoc: {max}");
            
            return max;
        }
    }
}
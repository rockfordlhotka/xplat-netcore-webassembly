int collatz(int value) {
  int count = 0;

  if(value > 1) {
    while(value > 1) {
      if(value % 2 == 0) {
        value = value / 2;
      }
      else {
        value = (3 * value) + 1;
      }
      
      count++;
    }
  }  
  return count;
}
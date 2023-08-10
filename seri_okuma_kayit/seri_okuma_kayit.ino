

const int max = 50;
char buf[max];
char paket[max];
int sayac1 = 0;
int sayac2 = 0;
int buff;
int g_mb;

void setup()
{
  Serial.begin(9600);
}
void loop()
{
  if(Serial.available() > 0)
  {
    buff = Serial.readBytesUntil('\n',buf,max);
    for(int a=0;a<buff;a++)
    {
      if(buf[a] == '*')
      {
      Serial.println("temizlendi");
      sayac1 = 0;
      memset(paket,0,sizeof(paket));
      }
    }
    Serial.println();
    g_mb = buff;
    kayit();
  }
}

void kayit()
{
 while(buff > 0)
  {
    paket[sayac1] = buf[sayac2];
    //Serial.println(sayac1);
    delay(50);
    sayac1++;
    sayac2++;
    buff--;
    if(g_mb < sayac2 + 1)
    {
      if(sayac1 < max)
      {
        kayit2();
      }
      else
      {
        Serial.println("Depo doldu");
        Serial.println("\tSilmek için gönderin = *");
      }
    }
  }
}
void kayit2()
{
  sayac2 = 0;
  paket[sayac1] = ',';
  sayac1 = sayac1+1;
  for(int a=0;a<sayac1;a++)
  {
    Serial.print(paket[a]);
    delay(50);
  }
}
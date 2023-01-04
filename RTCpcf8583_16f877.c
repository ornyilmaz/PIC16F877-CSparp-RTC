//Program: Analog Saat - MicroC 16f877-16f877A
//Programcı: Orhan YILMAZ
//E-mail : kralsam[AT]gmail.com
//web : www.mafgom.com
 
char seconds, minutes, hours, 
 
     day, month, year,
 
     en_kalibre = 0, send_en = 0;    // Global değişkenler
 
char tmph = 0, tmpm = 0, tmps = 0, temp = 0, tmpyd = 0, tmpmo = 0, k=0;
 
// Software I2C connections - Yazılımsal i2c Bağlantı Pinleri
 
sbit Soft_I2C_Scl           at RC3_bit;
 
sbit Soft_I2C_Sda           at RC4_bit;
 
sbit Soft_I2C_Scl_Direction at TRISC3_bit;
 
sbit Soft_I2C_Sda_Direction at TRISC4_bit;
 
// End Software I2C connections
 
// LCD module connections - LCD modül Bağlantı Pinleri
 
sbit LCD_RS at RD2_bit;
 
sbit LCD_EN at RD3_bit;
 
sbit LCD_D4 at RD4_bit;
 
sbit LCD_D5 at RD5_bit;
 
sbit LCD_D6 at RD6_bit;
 
sbit LCD_D7 at RD7_bit;
 
sbit LCD_RS_Direction at TRISD2_bit;
 
sbit LCD_EN_Direction at TRISD3_bit;
 
sbit LCD_D4_Direction at TRISD4_bit;
 
sbit LCD_D5_Direction at TRISD5_bit;
 
sbit LCD_D6_Direction at TRISD6_bit;
 
sbit LCD_D7_Direction at TRISD7_bit;
 
// End LCD module connections
 
//RTC Kalibrasyon - Write.. - RTC'ye yazma fonk..
 
void RTC_Kalibre()//char day, char mount, char year, char sec, char min, char hour)
 
{
 
 if(en_kalibre != 0)
 
 {
 
   Soft_I2C_Init();       // Initialize full master mode -- Protokolü kur
 
   Soft_I2C_Start();      // Issue start signal -- Haberleşme Başlatma Sinyali
 
   Soft_I2C_Write(0xA0);  // Address PCF8583, see PCF8583 datasheet -- Datasheet'e göre RTC Adres bilgisi
 
   Soft_I2C_Write(0);     // Start from address 0 (configuration memory location) -- Bellekteki Başlangıç Adresi(RTC)
 
   Soft_I2C_Write(0x80);  // Write 0x80 to configuration memory location (pause counter...) -- Saymayı durdur
 
   Soft_I2C_Write(0);     // Write 0 to cents memory location
 
   Soft_I2C_Write(tmps);//0);     // Write 0 to seconds memory location -- Saniye'yi Kalibre et(RTC'ye yaz..)
 
   Soft_I2C_Write(tmpm);//0x31);  // Write 0x30 to minutes memory location -- Dakika'yı Kalibre et(RTC'ye yaz..)
 
   Soft_I2C_Write(tmph);//0x21);  // Write 0x12 to hours memory location -- Saat'i Kalibre et(RTC'ye yaz..)
 
   Soft_I2C_Write(tmpyd);//0x12);  // Write 0x18 to year/date memory location -- Yıl ve Gün'ü Kalibre et(RTC'ye yaz..)
 
   Soft_I2C_Write(tmpmo);//0x12);  // Write 0x04 to weekday/month memory location -- Hafta ve Ay'ı Kalibre et(RTC'ye yaz..)
 
   Soft_I2C_Stop();       // Issue stop signal -- Haberleşme Bitiş Sinyali
 
   Soft_I2C_Start();      // Issue start signal -- Tekrar haberleşme başlatma sinyali
 
   Soft_I2C_Write(0xA0);  // Address PCF8530	-- RTC Adresi
 
   Soft_I2C_Write(0);     // Start from address 0 -- 0.Adres'e
 
   Soft_I2C_Write(0);     // Write 0 to configuration memory location (enable counting) -- 0 bilgisi yaz.. Saymayı etkinleştir.
 
   Soft_I2C_Stop();       // Issue stop signal -- Haberleşmeyi bitir.
 
   tmph = 0;
 
   tmpm = 0;
 
   tmps = 0;
 
   en_kalibre = 0; //Kalibrasyon bayrağını temizle..
 
  }
 
}
 
//--------------------- Reads time and date information from RTC (PCF8583) -- Bilgileri Okuma Fonk.
 
void Read_Time() {
 
  Soft_I2C_Start();               // Issue start signal
 
  Soft_I2C_Write(0xA0);           // Address PCF8583, see PCF8583 datasheet
 
  Soft_I2C_Write(2);              // Start from address 2
 
  Soft_I2C_Start();               // Issue repeated start signal
 
  Soft_I2C_Write(0xA1);           // Address PCF8583 for reading R/W=1
 
  seconds = Soft_I2C_Read(1);     // Read seconds byte
 
  minutes = Soft_I2C_Read(1);     // Read minutes byte
 
  hours = Soft_I2C_Read(1);       // Read hours byte
 
  day = Soft_I2C_Read(1);         // Read year/day byte
 
  month = Soft_I2C_Read(0);       // Read weekday/month byte
 
  Soft_I2C_Stop();                // Issue stop signal
 
}
 
//-------------------- Formats date and time -- Formatları Ayarlama Fonk.
 
void Transform_Time() {
 
  seconds  =  ((seconds & 0xF0) >> 4)*10 + (seconds & 0x0F);  // Transform seconds
 
  minutes  =  ((minutes & 0xF0) >> 4)*10 + (minutes & 0x0F);  // Transform months
 
  hours    =  ((hours & 0xF0)  >> 4)*10  + (hours & 0x0F);    // Transform hours
 
  year     =   ((day & 0xC0) >> 6) + 1 ;                             // Transform year
 
  day      =  ((day & 0x30) >> 4)*10    + (day & 0x0F);       // Transform day
 
  month    =  ((month & 0x10)  >> 4)*10 + (month & 0x0F);     // Transform month
 
}
 
//-------------------- Output values to LCD -- LCD'ye çıkış verme
 
void Display_Time() {
 
   Lcd_Chr(1, 7, (day / 10)   + 48);    // Print tens digit of day variable
 
   Lcd_Chr(1, 8, (day % 10)   + 48);    // Print oness digit of day variable
 
   Lcd_Chr(1,10, (month / 10) + 48);
 
   Lcd_Chr(1,11, (month % 10) + 48);
 
  // Lcd_Chr(1,15,  (year/10)        + 48);    // Print year variable  (start from year 2010)
 
   Lcd_Chr(1,16,  (year%10)        + 48);    // Print year variable  (start from year 2010)
 
   Lcd_Chr(2, 7, (hours / 10)   + 48);
 
   Lcd_Chr(2, 8, (hours % 10)   + 48);
 
   Lcd_Chr(2,10, (minutes / 10) + 48);
 
   Lcd_Chr(2,11, (minutes % 10) + 48);
 
   Lcd_Chr(2,13, (seconds / 10) + 48);
 
   Lcd_Chr(2,14, (seconds % 10) + 48);
 
}
 
//UART post -- rs232 haberleşme Fonk. (PC ye gönderim)
 
void Send_Uart() {
 
 if(seconds != send_en)
 
 {
 
   UART1_Write_Text("$SAAT,");
 
   UART1_Write((hours / 10)   + 48);
 
   UART1_Write((hours % 10)   + 48);
 
   UART1_Write((minutes / 10) + 48);
 
   UART1_Write((minutes % 10) + 48);
 
   UART1_Write((seconds / 10) + 48);
 
   UART1_Write((seconds % 10) + 48);
 
   UART1_Write_Text("nr");
 
   Delay_ms(20);
 
   UART1_Write_Text("$TARH,");
 
   UART1_Write((day / 10)   + 48);    // Print tens digit of day variable
 
   UART1_Write((day % 10)   + 48);    // Print oness digit of day variable
 
   UART1_Write((month / 10) + 48);
 
   UART1_Write((month % 10) + 48);
 
   UART1_Write_Text("201");
 
   UART1_Write( year + 48);    // Print year variable  (start from year 2010)
 
   UART1_Write_Text("nr");
 
   Delay_ms(20);
 
   send_en = seconds;
 
 }
 
}
 
//------------------ Performs project-wide init  -- Ana program başlangıç atamaları fonk.
 
void Init_Main() {
 
  TRISB = 0;
 
  PORTB = 0xFF;
 
  TRISB = 0xff;
 
  UART1_init(9600);
 
  Delay_ms(500);
 
  UART1_Write_Text("RTC Seriport hazir...nr");
 
  Soft_I2C_Init();           // Initialize Soft I2C communication
 
  Lcd_Init();                // Initialize LCD
 
  Lcd_Cmd(_LCD_CLEAR);       // Clear LCD display
 
  Lcd_Cmd(_LCD_CURSOR_OFF);  // Turn cursor off
 
  Lcd_Out(1,1,"Date:");      // Prepare and output static text on LCD
 
  Lcd_Chr(1,9,'.');
 
  Lcd_Chr(1,12,'.');
 
  //Lcd_Chr(1,16,'.');
 
  Lcd_Out(2,1,"Time:");
 
  Lcd_Chr(2,9,':');
 
  Lcd_Chr(2,12,':');
 
  Lcd_Out(1,13,"201");       // start from year 2010
 
}
 
//-
 
void main() {
 
  Init_Main();               // Perform initialization
 
  //en_kalibre = 1;
 
  while (1) {                // Sonsuz Döngü
 
    if(UART1_Data_Ready()) 
 
    {     // If data is received, -- Eğer data geldi ise(PC'den)
 
     switch(k) //Uygun sıraya göre değişkene al.
 
       {
 
        case 0:
 
             tmph = UART1_Read();
 
             break;
 
        case 1:
 
             tmpm = UART1_Read();
 
             break;
 
        case 2:
 
             tmps = UART1_Read();
 
             break;
 
        case 3:
 
             tmpyd = UART1_Read();
 
             break;
 
        case 4:
 
             tmpmo = UART1_Read();
 
             break;
 
        }
 
       k++;
 
       k%=5;
 
       if(k == 0)
 
            en_kalibre = 1; //Kalibrasyon Bayrağını aktif et
 
     }
 
    RTC_Kalibre();
 
    Read_Time();             // Read time from RTC(PCF8583)
 
    Transform_Time();        // Format date and time
 
    Display_Time();          // Prepare and display on LCD
 
    Send_Uart();
 
  }
 
}

//Program: Analog Saat - C#
//Programcı: Orhan YILMAZ
//E-mail : kralsam[AT]gmail.com
//web    : www.mafgom.com
 
using System;
 
using System.Drawing;
 
using System.Windows.Forms;
 
using System.IO.Ports;
 
public class Saatform:Form
 
{
 
	private Graphics g;
 
	private Brush br1,br2;
 
	private Pen p1,pbgh,pbgm,pbgs,ph,pm,ps;
 
	//private PictureBox picBg;
 
	private Button b1;
 
	public Label l1,l2,lsa,ltar;
 
	private Timer t;
 
	private int send_timer=0;
 
	// Seri port ilave edelim
 
	public SerialPort sp;
 
	private string rx1="",rx2="";
 
	private Byte [] gb = new Byte[5];
 
	private int SCap = 200; // Saat çapi
 
	private int Local = 40; //Saat Konumu
 
	private int SPC = 8; // Saat degerlerinin daire çapi
 
	private int DPC = 6; // Dakika deg. daire capi
 
	private int HLW = 7; //Saat çubuk kalinligi..
 
	private int MLW = 5; //Dakika
 
	private int SLW = 2;
 
	public int sec = 0,min = 0, ho = 0,day = 0, month = 0, year = 0;
 
	private float sx1e = 0,
 
			sy1e = 0,
 
			sx2e = 0,
 
			sy2e = 0,
 
			sx1y = 0,
 
			sy1y = 0,
 
			sx2y = 0,
 
			sy2y = 0;
 
	private float mx1e = 0,
 
			my1e = 0,
 
			mx2e = 0,
 
			my2e = 0,
 
			mx1y = 0,
 
			my1y = 0,
 
			mx2y = 0,
 
			my2y = 0;
 
	private float hx1e = 0,
 
			hy1e = 0,
 
			hx2e = 0,
 
			hy2e = 0,
 
			hx1y = 0,
 
			hy1y = 0,
 
			hx2y = 0,
 
			hy2y = 0;
 
	public Saatform()
 
	{
 
		Size 	= new Size(300,330);
 
		Text 	= "Analog Saat";
 
		b1 	= new Button();
 
		b1.Location = new Point(115,260);
 
		b1.Size = new Size(50,20);
 
		b1.Text = "Ayar";
 
		Controls.Add(b1);
 
		b1.Click += new EventHandler(AyarButton);
 
		pbgh	= new Pen(this.BackColor,HLW);
 
		pbgm	= new Pen(this.BackColor,MLW);
 
		pbgs	= new Pen(this.BackColor,SLW);
 
		p1	= new Pen(Color.White, 3);
 
		ph 	= new Pen(Color.Black, HLW);
 
		pm 	= new Pen(Color.Black, MLW);
 
		ps 	= new Pen(Color.Red, SLW);
 
		br1 	= new SolidBrush(Color.Black);
 
		br2	= new SolidBrush(Color.Green);
 
		l1	= new Label();
 
		l1.Location = new Point(80,10);
 
		l1.Size = new Size(200,20);
 
		l1.Text = "Analog Saat Çalismasi";
 
		Controls.Add(l1);
 
		l2	= new Label();
 
		l2.Location = new Point(200,270);
 
		l2.Size = new Size(200,20);
 
		l2.ForeColor = Color.Red;
 
		l2.Text = "";
 
		Controls.Add(l2);
 
		lsa	= new Label();
 
		lsa.Location = new Point(10,250);
 
		lsa.Size = new Size(200,20);
 
		lsa.ForeColor = Color.Red;
 
		lsa.Text = "";
 
		Controls.Add(lsa);
 
		ltar	= new Label();
 
		ltar.Location = new Point(10,270);
 
		ltar.Size = new Size(200,20);
 
		ltar.ForeColor = Color.Red;
 
		ltar.Text = "";
 
		Controls.Add(ltar);
 
		// Seri port
 
		// Seri portu tanýmlayalým
 
		sp = new SerialPort("COM1",9600);
 
		sp.DataReceived += new SerialDataReceivedEventHandler(getData);
 
		sp.Open();
 
		/*
 
		picBg = new PictureBox();
 
		picBg.SizeMode = PictureBoxSizeMode.StretchImage;
 
		picBg.Location = new Point(40,40);
 
		picBg.Size = new Size(200,200);
 
		picBg.Image = new Bitmap("bgclock2.png");
 
		picBg.BorderStyle = BorderStyle.Fixed3D;
 
		Controls.Add(picBg);
 
		picBg.Paint += new PaintEventHandler(picBgPaint);
 
		*/
 
		gb[0] = 0;
 
		gb[1] = 0;
 
		gb[2] = 0;
 
		gb[3] = 0;
 
		gb[4] = 0;
 
		t = new Timer();
 
		t.Interval = 70;
 
		t.Enabled = false;
 
		t.Tick += new EventHandler(timer);
 
	}
 
	public void getData(object s, SerialDataReceivedEventArgs e)
 
	{
 
		sp = (SerialPort) s;
 
		rx1 = sp.ReadLine();
 
		rx2 = sp.ReadLine();
 
		try{
 
		//if(tmp2[0] == "$TARH")
 
		//{
 
			ltar.Text = "Tarih: " + rx2.Substring(7,2) + "." + rx2.Substring(9,2) + "." + rx2.Substring(11,4);
 
			//l2.Text = tmp2[0];//"Saat:  " + rx2.Substring(7,2) + ":" + rx2.Substring(9,2) + ":" + rx2.Substring(11,2);			
 
			day = int.Parse(rx2.Substring(7,2));
 
			month = int.Parse(rx2.Substring(9,2));
 
			year = int.Parse(rx2.Substring(14,1));
 
			rx2="";
 
		//}
 
		}catch{}
 
		try{	
 
		//if(tmp1[0] == "$SAAT")
 
		//{
 
			lsa.Text = "Saat:  " + rx1.Substring(7,2) + ":" + rx1.Substring(9,2) + ":" + rx1.Substring(11,2);
 
			sec = int.Parse(rx1.Substring(11,2));
 
			sec %=60;
 
			min = int.Parse(rx1.Substring(9,2))*59 + sec;
 
			min %= 60*60;
 
			ho = int.Parse(rx1.Substring(7,2));//*60*60;
 
			ho = ho*59*10 + min/6;
 
			//ho %= 60*60/5;
 
			Hour();
 
			Minute();
 
			Second();
 
			rx1="";
 
		//}
 
		}catch{}
 
		//l.Text = //"Deðer:  " + vDegeri.ToString("f2") + "t V";
 
	}
 
	protected override void OnPaint(PaintEventArgs e)
 
	{
 
		g = e.Graphics;
 
		g.DrawEllipse(p1,Local,Local,SCap,SCap); //Saat cerceve..
 
		for(int i =0; i < 12; i++) //Saat ..
 
		{
 
			g.FillEllipse(br1,Local+(SCap/2)+(SCap/2)*((float)Math.Sin(i*30*Math.PI/180))-(SPC/2),Local+(SCap/2)+(SCap/2)*((float)Math.Cos(i*30*Math.PI/180))-(SPC/2),SPC,SPC);
 
		}
 
		for(int j=0; j<60 ; j++)
 
			if((j%5) != 0)
 
				g.FillEllipse(br2,Local+(SCap/2)+(SCap/2)*((float)Math.Sin(j*6*Math.PI/180))-(DPC/2),Local+(SCap/2)+(SCap/2)*((float)Math.Cos(j*6*Math.PI/180))-(DPC/2),DPC,DPC);
 
		sx1e = Local+(SCap/2);
 
		sy1e = Local+(SCap/2);
 
		sx2e = Local+(SCap/2);
 
		sy2e = Local+10;
 
		//g.DrawLine(ps, sx1e, sy1e, sx2e, sy2e);
 
		g.FillEllipse(br1,Local+(SCap/2)-3,Local-3+(SCap/2),6,6);
 
	}
 
/*	private void picBgPaint(Object s, PaintEventArgs e)
 
	{
 
		Second();
 
		Minute();
 
		Hour();
 
	}
 
*/	
 
	private void AyarButton(Object s, EventArgs e)
 
	{
 
		//t.Enabled = false;
 
		Ayar a = new Ayar((int)(ho/(600)), (int)(min/(60)), (int)(sec),day,month,year);
 
		a.f1=this;
 
		a.Show();
 
	}
 
	public void Kalibre(int hour, int minute, int second, int day, int month, int year)
 
	{
 
		if(year !=0)
 
			year--;
 
		//sp.WriteLine("$SAAT" + hour.ToString("d2") + minute.ToString("d2") + second.ToString("d2") +"&");
 
		gb[0] = (byte)( (hour/10)*0x10 + (hour%10));
 
		gb[1] = (byte)( (minute/10)*0x10 + (minute%10));
 
		gb[2] = (byte)( (second/10)*0x10 + (second%10));
 
		gb[3] = (byte)( ( (year%10) )*0x40 + (day/10)*0x10 + (day%10)); // XXxx xxxx-year , xxXX XXXX-day
 
		gb[4] = (byte)( (month/10)*0x10 + (month%10) );
 
		t.Enabled = true;
 
	}
 
	public void timer(object sender, EventArgs e) //Kalibre için..
 
	{
 
		switch(send_timer)
 
		{
 
			case 0:
 
				sp.Write(gb, 0, 1);
 
				break;
 
			case 1:
 
				sp.Write(gb, 1, 1);
 
				break;
 
			case 2:
 
				sp.Write(gb, 2, 1);
 
				break;
 
			case 3:
 
				sp.Write(gb, 3, 1);
 
				break;
 
			case 4:
 
				sp.Write(gb, 4, 1);
 
				break;
 
		}
 
		send_timer++;
 
		if(send_timer == 5)
 
		{
 
			t.Enabled = false;
 
			send_timer = 0;
 
		}
 
	}
 
	private void Second()
 
	{
 
		sx1y = Local+(SCap/2);
 
		sy1y = Local+(SCap/2);
 
		sx2y = Local+(SCap/2)+(SCap/2-10)*((float)Math.Sin(sec*6*Math.PI/180));
 
		sy2y = Local+(SCap/2)-(SCap/2-10)*((float)Math.Cos(sec*6*Math.PI/180));
 
		g =  this.CreateGraphics();
 
		g.DrawLine(pbgs, sx1e, sy1e, sx2e, sy2e);
 
		g.DrawLine(ps, sx1y, sy1y, sx2y, sy2y);
 
		g.FillEllipse(br1,Local+(SCap/2)-3,Local-3+(SCap/2),6,6);
 
		sx1e = sx1y;
 
		sy1e = sy1y;
 
		sx2e = sx2y;
 
		sy2e = sy2y;
 
	}
 
	private void Minute()
 
	{
 
		mx1y = Local+(SCap/2);
 
		my1y = Local+(SCap/2);
 
		mx2y = Local+(SCap/2)+(SCap/2-30)*((float)Math.Sin(min*0.1*Math.PI/180));
 
		my2y = Local+(SCap/2)-(SCap/2-30)*((float)Math.Cos(min*0.1*Math.PI/180));
 
		g =  this.CreateGraphics();
 
		g.DrawLine(pbgm, mx1e, my1e, mx2e, my2e);
 
		g.DrawLine(pm, mx1y, my1y, mx2y, my2y);
 
		g.FillEllipse(br1,Local+(SCap/2)-3,Local-3+(SCap/2),6,6);
 
		mx1e = mx1y;
 
		my1e = my1y;
 
		mx2e = mx2y;
 
		my2e = my2y;
 
	}
 
	private void Hour()
 
	{
 
		hx1y = Local+(SCap/2);
 
		hy1y = Local+(SCap/2);
 
		hx2y = Local+(SCap/2)+(SCap/2-50)*((float)Math.Sin(ho*(0.05)*Math.PI/180));
 
		hy2y = Local+(SCap/2)-(SCap/2-50)*((float)Math.Cos(ho*(0.05)*Math.PI/180));
 
		g =  this.CreateGraphics();
 
		g.DrawLine(pbgh, hx1e, hy1e, hx2e, hy2e);
 
		g.DrawLine(ph, hx1y, hy1y, hx2y, hy2y);
 
		g.FillEllipse(br1,Local+(SCap/2)-3,Local-3+(SCap/2),6,6);
 
		hx1e = hx1y;
 
		hy1e = hy1y;
 
		hx2e = hx2y;
 
		hy2e = hy2y;
 
	}
 
}
 
public class Ayar:Form
 
{
 
	public Saatform f1;
 
	public TextBox txt1, txt2, txt3,txd1,txd2,txd3;
 
	private Button b1, b2;
 
	private Label lt1,lt2,lt3,ld1,ld2,ld3,bilgi,bil2;
 
	public Ayar(int ho,int min,int sec,int day,int month,int year)
 
	{
 
		Size = new Size(300,155);
 
		Text = "Saat Ayar Formu";
 
		bilgi = new Label();
 
		bilgi.Location = new Point(20,5);
 
		bilgi.Size = new Size(250,20);
 
		bilgi.ForeColor = Color.Red;
 
		bilgi.Text = "12'li yada 24'lü sisteme göre giris yapabilirsiniz";
 
		Controls.Add(bilgi);
 
		bil2 = new Label();
 
		bil2.Location = new Point(210,62);
 
		bil2.Size = new Size(250,20);
 
		bil2.ForeColor = Color.Red;
 
		bil2.Text = "En çok 2014 !";
 
		Controls.Add(bil2);
 
		lt1 = new Label();
 
		lt1.Location = new Point(30,32);
 
		lt1.Size = new Size(31,20);
 
		lt1.Text = "Saat:";
 
		Controls.Add(lt1);
 
		lt2 = new Label();
 
		lt2.Location = new Point(85,32);
 
		lt2.Size = new Size(43,20);
 
		lt2.Text = "Dakika:";
 
		Controls.Add(lt2);
 
		lt3 = new Label();
 
		lt3.Location = new Point(150,32);
 
		lt3.Size = new Size(42,20);
 
		lt3.Text = "Saniye:";
 
		Controls.Add(lt3);
 
		txt1 = new TextBox();
 
		txt1.Location = new Point(61, 30);
 
		txt1.Size = new Size(20, 20);
 
		txt1.Text = ho.ToString("d2");//"";
 
		Controls.Add(txt1);
 
		txt2 = new TextBox();
 
		txt2.Location = new Point(128, 30);
 
		txt2.Size = new Size(20,20);
 
		txt2.Text = min.ToString("d2");//"";
 
		Controls.Add(txt2);
 
		txt3 = new TextBox();
 
		txt3.Location = new Point(194, 30);
 
		txt3.Size = new Size(20,20);
 
		txt3.Text = sec.ToString("d2");//"";
 
		Controls.Add(txt3);
 
		ld1 = new Label();
 
		ld1.Location = new Point(30,62);
 
		ld1.Size = new Size(31,20);
 
		ld1.Text = "Gün:";
 
		Controls.Add(ld1);
 
		ld2 = new Label();
 
		ld2.Location = new Point(85,62);
 
		ld2.Size = new Size(43,20);
 
		ld2.Text = "      Ay:";
 
		Controls.Add(ld2);
 
		ld3 = new Label();
 
		ld3.Location = new Point(150,62);
 
		ld3.Size = new Size(45,20);
 
		ld3.Text = "Yýl:  201";
 
		Controls.Add(ld3);
 
		txd1 = new TextBox();
 
		txd1.Location = new Point(61, 60);
 
		txd1.Size = new Size(20, 20);
 
		txd1.Text = day.ToString("d2");//"";
 
		Controls.Add(txd1);
 
		txd2 = new TextBox();
 
		txd2.Location = new Point(128, 60);
 
		txd2.Size = new Size(20,20);
 
		txd2.Text = month.ToString("d2");//"";
 
		Controls.Add(txd2);
 
		txd3 = new TextBox();
 
		txd3.Location = new Point(196, 60);
 
		txd3.Size = new Size(13,20);
 
		txd3.Text = year.ToString("d1");//"";
 
		Controls.Add(txd3);
 
		b1 = new Button();
 
		b1.Location = new Point(20,90);
 
		b1.Text = "Kaydet";
 
		b1.Size = new Size(60,20);
 
		Controls.Add(b1);
 
		b1.Click += new EventHandler(kaydet);
 
		b2 = new Button();
 
		b2.Location = new Point(90,90);
 
		b2.Text = "Kapat";
 
		b2.Size = new Size(60,20);
 
		Controls.Add(b2);
 
		b2.Click += new EventHandler(kapat);
 
	}
 
	private void kaydet(Object s, EventArgs e)
 
	{
 
		//Saatform 'da
 
		/*
 
		f1.ho = (600*60)*(((int)double.Parse(txt1.Text))%24);
 
		f1.min = 600*((int)double.Parse(txt2.Text)%60);
 
		f1.sec = 10*((int)double.Parse(txt3.Text)%60);
 
		*/
 
		f1.Kalibre(((int.Parse(txt1.Text))%24), (int.Parse(txt2.Text)%60), (int.Parse(txt3.Text)%60), (int.Parse(txd1.Text)),(int.Parse(txd2.Text)),(int.Parse(txd3.Text)));
 
		f1.l2.Text = "Kalibre Edildi !";
 
	}
 
	private void kapat(Object s, EventArgs e)
 
	{
 
		this.Close();
 
	}
 
}
 
public class Driver
 
{
 
	public static void Main()
 
	{
 
		Saatform f1 = new Saatform();
 
		Application.Run(f1);
 
	}
 
}

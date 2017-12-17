using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace Hangman
{
    #region Vung khai bao kieu du lieu
    // cau truc luu tru mot thanh tich
    struct ThanhTich
    {
        public string ten; // ho ten nguoi choi
        public double sogiay; // thoi gian choi bang giay
    }

    // Kieu cu phap the hien cu phap dong lenh khi goi chuong trinh
    // Dang 1 : Hangman.exe
    // Dang 2 : Hangman.exe -h
    // Dang 3 : Hangman.exe <path>
    enum KieuCuPhap { Dang1, Dang2, Dang3 };
    #endregion

    class Program
    {
        #region Khai bao bien
        // duong dan den tap tin tu dien tu
        static string dictpath = "words.txt";
        // danh sach cac tu doc duoc trong tu dien
        static List<string> listWords;
        // tu duoc chon de choi
        static string secret_word;
        // cac ky tu da doan
        static List<char> letters_guessed;
        // so lan da doan sai
        static int mistakes_made;
        // bien kieuCP de nhan dang cu phap dong lenh
        static KieuCuPhap kieuCP;
        // duong dan den tap tin luu danh sach vinh danh
        static string top10path = "top10.txt";
        // bien luu tru danh sach top10
        static List<ThanhTich> listTop10;
        // thanh tich moi
        static ThanhTich thanhtich;
        #endregion

        #region Main
        static void Main(string[] args)
        {
            // thiet lap lai kich thuoc cua console
            Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);

            // Kiem tra doi so dong lenh
            // de xac dinh dang cu phap dong lenh
            if (args.Length == 0)
                kieuCP = KieuCuPhap.Dang1; // khi khong co doi so nao thi thuc hien theo dang 1
            else if (args.Length == 1)
            { // khi co 1 doi so, co the thuoc vao dang 2, 3
                if (args[0] == "-h") // neu la chuoi "-h"
                    kieuCP = KieuCuPhap.Dang2; // thuoc dang 2
                else // nguoc lai, thuoc dang 3
                    kieuCP = KieuCuPhap.Dang3;
            }
            else
            {
                // In thong bao loi va hien thi huong dan su dung
                Console.WriteLine("Lenh goi chuong trinh cua ban bi sai");
                kieuCP = KieuCuPhap.Dang2;
            }

            // xu ly theo dang cu phap dong lenh
            switch (kieuCP)
            {
                case KieuCuPhap.Dang2:
                    InHuongDanSuDung(); // In ra huong dan su dung
                    Console.ReadKey(); // Cho nguoi sd doc huong dan va bam enter
                    Environment.Exit(0); // roi thoat khoi chuong trinh
                    break;
                case KieuCuPhap.Dang3:
                    // Goi ham nhap du lieu voi so luong phan tu da biet
                    dictpath = args[0];
                    break;
            }

            // Doc file tu dien
            if (!DocTuDien())
                return; // neu doc khong duoc

            // khoi tao va doc file top10.txt
            listTop10 = new List<ThanhTich>();
            DocTop10();

            // Doc duoc thi hien thi menu
            XuLyMenu();
        }

        // Ham thuc hien in ra huong dan su dung cho chuong trinh
        static void InHuongDanSuDung()
        {
            Console.WriteLine();
            Console.WriteLine("TRO CHOI HANGMAN");
            Console.WriteLine("Cu phap cau lenh theo 1 trong 3 dang sau :");
            Console.WriteLine("Hangman.exe"); // Dang 1
            Console.WriteLine("Hangman.exe -h"); // Dang 2
            Console.WriteLine("Hangman.exe <path>"); // Dang 4
            Console.WriteLine();
        }
        #endregion

        #region Xu ly menu
        // In menu cua chuong trinh
        static void InMenu()
        {
            Console.WriteLine();
            Console.Write("TRO CHOI HANGMAN");
            Console.WriteLine();
            Console.WriteLine("1 - Choi van moi");
            Console.WriteLine("2 - Xem danh sach vinh danh");
            Console.WriteLine("3 - Thoat");
            Console.WriteLine("Hay chon so tu 1 den 3 phu hop voi chuc nang tuong ung.");
            Console.Write("Ban chon so : ");
        }

        // ham xu ly menu
        static void XuLyMenu()
        {
            int menu;
            while (true)
            {
                InMenu(); // in menu ra man hinh
                menu = XulyChonMenu(1, 3); // cho nguoi su dung chon menu

                switch (menu)
                {
                    case 1: // Choi van moi
                        Choi();
                        break;
                    case 2: // Xem danh sach vinh danh
                        InTop10();
                        break;
                    case 3: // thoat khoi chuong trinh
                        if (XuLyCauHoiYesNo("Ban co chac muon thoat chuong trinh ? (Y/N): "))
                        {
                            Console.WriteLine("\nCam on ban da su dung chuong trinh !");
                            Console.ReadKey();

                            // luu danh sach vinh danh truoc khi ket thuc chuong trinh
                            LuuTop10();
                            Environment.Exit(0);
                        }
                        break;
                }
            }
        }

        // ham nay tra ra mot so tu 1 den 3
        // tuong ung voi cac so tren menu ma nguoi su dung nhap
        // hai tham so soduoi va sotren giup cho ham nay co the xu ly cho bat ky menu nao
        static int XulyChonMenu(int soduoi, int sotren)
        {
            int menu;
            // ham TryParse thuc hien chuyen doi tu chuoi sang so
            // neu co the chuyen duoc thi ham nay tra ra gia tri true
            // va ket qua o tham chieu menu
            while (!int.TryParse(Console.ReadLine(), out menu)
                   && ((menu < soduoi) || (menu > sotren)))
            {
                Console.WriteLine("Ban chon so nam ngoai pham vi cac so tren menu.");
                Console.WriteLine("Hay chon so tu {0} den {1} phu hop voi chuc nang tuong ung.", soduoi, sotren);
                Console.Write("Moi ban chon lai : ");
            }
            return menu;
        }
        #endregion

        #region Hangman
        static void Choi()
        {
            // Lay mot tu ngau nhien tu tu dien
            secret_word = ChonTuDeChoi();
            if (secret_word == null) // neu loi
            {
                ThongBaoLoi("Khong the lua chon tu de choi.");
                return;
            }

            // Khoi gan lai gia tri cho letters_guessed va mistakes_made
            KhoiGanLaiGiaTri();
            // lay ten nguoi choi
            Console.Write("Moi ban nhap ten nguoi choi : ");
            thanhtich.ten = Console.ReadLine();

            DateTime tgBatDauChoi = DateTime.Now; // lay thoi gian bat dau choi

            TrangThai trangthai = TrangThai.Choi;
            string mess = string.Format("TU can doan co {0} ky tu: ", secret_word.Length);
            while (true)
            {
                InTrangThaiHienTai(trangthai, mess);
                if (trangthai == TrangThai.Thang) // tinh thoi gian thang
                {
                    TimeSpan tongThoiGianChoi = DateTime.Now - tgBatDauChoi;
                    thanhtich.sogiay = tongThoiGianChoi.TotalSeconds;

                    // so sanh xem thoi gian nay co nam trong pham vi top 10 khong
                    // neu co thi chen vao, xoa cuoi
                    // luu xuong file truoc khi ket thuc chuong trinh
                    ThemThanhTich();
                }
                if (trangthai == TrangThai.Thang || trangthai == TrangThai.Thua)
                    break;
                // Cho nguoi dung nhap vao ky tu hoac tu can doan
                string doan = NhapDuDoan();
                Console.Clear();

                if (doan.Length == 1) // doan 1 ky tu
                {
                    // Kiem tra xem ky tu do da doan hay chua
                    if (LaDaDoan(doan[0]))
                    {
                        mess = string.Format("Ban da doan ky tu \'{0}\' roi : ", doan[0]);
                        continue;
                    }

                    char c = doan[0];
                    letters_guessed.Add(c);
                    if (LaTrung())
                    {
                        trangthai = TrangThai.Thang;
                        mess += string.Format("THANG ROI. Chuc mung ban!\nTu ban doan duoc la : {0}", secret_word);
                    }
                    else
                    {
                        // Kiem tra tu moi doan co dung khong
                        if (LaCo(c))
                        {
                            mess = string.Format("Co {0} ky tu \'{1}\' trong tu can doan : ",
                                                 DemKyTu(c),
                                                 c);
                        }
                        else // neu khong co
                        {
                            mistakes_made++; // tang so loi
                            if (mistakes_made < 6)
                                mess = string.Format("Khong co ky tu \'{0}\' trong tu can doan : ", c);
                            else if (mistakes_made == 6) // neu da du 6 loi
                            {
                                trangthai = TrangThai.Thua;
                                mess = string.Format("Khong co ky tu \'{0}\' trong tu can doan.\n", c);
                                mess += "BAN THUA! Ban da co 6 lan doan sai.\n";
                                mess += string.Format("Tu can doan la {0}\n", secret_word);
                                // in ra cac ky tu chua doan duoc
                                mess += string.Format("Cac ky tu ban chua doan duoc : {0}", CacKyTuChuaDoanDuoc());
                            }
                        }
                    }
                }
                else // mot tu
                {
                    if (LaTrung(doan))
                    {
                        trangthai = TrangThai.Thang;
                        mess += string.Format("\nTHANG ROI. Chuc mung ban!\nTu ban doan duoc la : {0}", secret_word);
                    }
                    else
                    {
                        mistakes_made += 2;
                        mess = string.Format("Khong phai la tu {0} dau ban.", doan);
                        if (mistakes_made >= 6)
                        {
                            trangthai = TrangThai.Thua;
                            mess += "BAN THUA! Ban da co 6 lan doan sai.\n";
                            mess += string.Format("Tu can doan la {0}\n", secret_word);
                            // in ra cac ky tu chua doan duoc
                            mess += string.Format("Cac ky tu ban chua doan duoc : {0}", CacKyTuChuaDoanDuoc());
                        }
                    }
                }
            }
        }

        enum TrangThai { Choi, Thang, Thua}
        // Ham in trang thai hien tai cua tro choi
        static void InTrangThaiHienTai(TrangThai trangthai, string mess)
        {
            Console.Clear();
            //Console.WriteLine(secret_word); // XOA SAU KHI XONG CHUONG TRINH
            Console.WriteLine("TRO CHOI HANGMAN");
            HangmanImage.PrintImage(mistakes_made);
            Console.WriteLine();
            switch (trangthai)
            {
                case TrangThai.Choi:
                    Console.Write(mess);
                    // In chuoi ky tu can doan
                    Console.WriteLine(ChuoiKyTuCanDoan());
                    // Thong bao cac ky tu da du doan
                    Console.WriteLine("Ban da doan cac ky tu : {0}", CacKyTuDaDuDoan());
                    // Thong bao so lan con co the du doan
                    Console.WriteLine("Ban con {0} lan doan.", 6 - mistakes_made);
                    break;
                case TrangThai.Thang:
                case TrangThai.Thua:
                    Console.WriteLine(mess);
                    Console.ReadKey();
                    break;
            }
        }

        // Ham nhap tu hoac ky tu du doan
        static string NhapDuDoan()
        {
            Console.Write("Moi nhap ky tu hoac tu du doan: ");
            string doan = Console.ReadLine();
            while (true)
            {
                if (string.IsNullOrEmpty(doan))
                {
                    Console.Write("Ky tu hoac tu khong the la rong. Moi ban nhap lai: ");
                    doan = Console.ReadLine();
                    continue;
                }
                if (doan.Length == 1 && !char.IsLetter(doan[0]))
                {
                    Console.Write("Ban phai nhap ky tu. Moi ban nhap lai: ");
                    doan = Console.ReadLine();
                    continue;
                }
                break;
            }
            return doan;
        }

        // Ham kiem tra cac ky tu da doan trung voi tu can doan
        static bool LaTrung()
        {
            foreach (char c in secret_word)
                if (!letters_guessed.Contains(c))
                    return false;
            return true;
        }

        // Ham kiem tra trung 1 tu
        static bool LaTrung(string st)
        {
            return secret_word == st;
        }

        // Ham kiem tra ky tu da co trong letters_guessed hay khong
        static bool LaDaDoan(char c)
        {
            return letters_guessed.Contains(c);
        }

        // Ham kiem tra ky tu doan co ton tai trong tu du doan khong
        static bool LaCo(char c)
        {
            return secret_word.Contains(c);
        }

        // Ham dem so ky tu trong tu can doan
        static int DemKyTu(char c)
        {
            int dem = 0;
            foreach (char t in secret_word)
                if (c == t) dem++;
            return dem;
        }

        // Ham tra ra cac ky tu da du doan
        static string CacKyTuDaDuDoan()
        {
            string st = "";
            foreach (char c in letters_guessed)
                st += string.Format(" {0}", c);
            return st;
        }

        // Ham tra ra ky tu chua doan duoc
        static string CacKyTuChuaDoanDuoc()
        {
            string st = "";
            foreach (char c in secret_word)
                if (!letters_guessed.Contains(c))
                    st += string.Format(" {0}", c);
            return st;
        }

        // Ham in day ky tu can doan
        static string ChuoiKyTuCanDoan()
        {
            string st = "";
            foreach (char c in secret_word)
                if (letters_guessed.Contains(c))
                    st += c;
                else
                    st += "-";
            return st;
        }

        // Ham lua chon 1 tu de choi
        static string ChonTuDeChoi()
        {
            // neu chua co danh sach tu
            if (listWords == null || listWords.Count == 0)
                return null; // thi tra ra null

            Random r = new Random();
            int index = r.Next(0, listWords.Count); // lay mot so ngau nhien
            return listWords[index]; // tra ra tu tai vi tri ngau nhien do
        }

        // Gan lai gia tri ban dau cho cac bien luu tru du lieu
        static void KhoiGanLaiGiaTri()
        {
            letters_guessed = new List<char>();
            mistakes_made = 0;
            thanhtich = new ThanhTich();
        }
        #endregion

        #region Top10
        // In danh sach vinh danh
        static void InTop10()
        {
            Console.Clear();
            Console.WriteLine("BANG VINH DANH");
            int count = 0;
            foreach(ThanhTich tt in listTop10)
            {
                count++;
                TimeSpan time = TimeSpan.FromSeconds((long)tt.sogiay);
                Console.WriteLine("{0}. {1} - {2}:{3}:{4}",
                                  count,
                                  tt.ten,
                                  time.Hours,
                                  time.Minutes,
                                  time.Seconds);
            }
        }

        // Them thanh tich moi vao top10
        // co the them duoc hoac khong
        static void ThemThanhTich()
        {
            int i;
            for (i = 0; i < listTop10.Count; i++)
                if (listTop10[i].sogiay > thanhtich.sogiay)
                    break;
            if (i == listTop10.Count && listTop10.Count < 10)
                listTop10.Add(thanhtich);
            if (i < listTop10.Count)
                listTop10.Insert(i, thanhtich);
            if (listTop10.Count > 10)
                listTop10.RemoveAt(listTop10.Count - 1);
        }
        #endregion

        #region Xu ly file
        static bool DocTuDien()
        {
            if (dictpath == null) // neu khong co duong dan den tu dien
            {
                ThongBaoLoiFile("Ban chua cung cap tu dien tu.");
                return false; // thi thoat khoi ham
            }

            // mo file ra de doc
            StreamReader reader = new StreamReader(dictpath);
            if (reader == null) // neu mo bi loi
            {
                ThongBaoLoiFile("File ban nhap khong ton tai hoac duong dan sai.");
                return false; // thi thoat khoi ham
            }

            // doc tat ca cac tu
            string dong = reader.ReadLine();
            if (dong == null) // neu doc khong duoc
            {
                ThongBaoLoiFile("File tu dien bi loi.");
                return false; // thoat khoi ham
            }

            if (!TachTu(dong)) // thuc hien tach tu
            {
                listWords = null; // tach khong duoc
                return false;
            }
            
            // Dong tap tin
            reader.Close();

            return true;
        }

        // Ham co nhiem vu tach cac tu da doc duoc tu tu dien
        static bool TachTu(string input)
        {
            if (input == null)
            {
                ThongBaoLoiFile("File tu dien bi loi.");
                return false;
            }

            listWords = new List<string>();

            string[] items = input.Split(new char[]{' '}, StringSplitOptions.RemoveEmptyEntries);
            listWords.AddRange(items);

            return true;
        }

        // Ham doc file top10.txt
        static void DocTop10()
        {
            // kiem tra xem file top10.txt da co hay chua
            if (!File.Exists(top10path)) // neu chua thi thoat, khong can doc
                return;

            // neu roi thi mo ra doc
            StreamReader reader = new StreamReader(top10path);
            if (reader == null) // neu mo bi loi
                return; // thi thoat khoi ham

            // neu loi trong qua trinh doc va xu ly thi khong doc nua
            // danh sach da doc duoc cu de vay
            string dong = null;
            // doc mot dong trong file
            while ((dong = reader.ReadLine()) != null) // neu het file thi khong doc nua
            {
                ThanhTich tt;
                // tach cac thanh phan trong dong doc duoc
                // va gan gia tri vao bien cau truc don
                if (!TachGiaTri(dong, out tt)) // neu qua trinh tach bi loi
                    return; // thoat khoi ham

                // nguoc lai
                listTop10.Add(tt); // them thanh tich vao danh sach
                // file da luu theo thu tu so giay tang dan
                // nen khi doc ra chi can dung ham Add
            }

            // Dong tap tin
            reader.Close();
        }

        // Ham co nhiem vu tach chuoi doc duoc tu file top10.txt
        // gan gia tri vao bien tt
        static bool TachGiaTri(string input, out ThanhTich tt)
        {
            tt = new ThanhTich();
            string[] items = input.Split('|'); // tach chuoi

            // phai chua 2 thanh phan
            if (items.Length != 2) return false;

            tt.ten = items[0]; // thanh phan thu nhat gan cho ten

            // thanh phan thu hai
            // ep kieu sang so kieu double
            if (!double.TryParse(items[1], out tt.sogiay))
                return false; // neu ep khong duoc thi bao loi

            return true;
        }

        // Xu ly luu danh sach top10 xuong file
        static void LuuTop10()
        {
            while (string.IsNullOrEmpty(top10path)) // neu khong co duong dan
                top10path = "top10.txt";

            // mo file ra de ghi
            StreamWriter writer = new StreamWriter(top10path);
            if (writer == null) // mo khong duoc
                return; // error

            // Ghi dữ liệu vào tập tin
            foreach (ThanhTich tt in listTop10)
                writer.WriteLine("{0}|{1}",
                                 tt.ten,
                                 tt.sogiay);

            writer.Close(); // dong file
        }

        // Ham xu ly in thong bao cac loi khi doc va ghi file
        static void ThongBaoLoiFile(string st, StreamReader reader = null)
        {
            ThongBaoLoi(st);
            if (reader != null)
                reader.Close();
        }
        #endregion

        #region CHUNG
        // Ham xu ly in cau hoi yes no
        static bool XuLyCauHoiYesNo(string cauhoi)
        {
            Console.Write(cauhoi); // in cau hoi
            ConsoleKeyInfo c = Console.ReadKey(); // nguoi su dung nhap vao lua chon
            Console.WriteLine();

            if (c.KeyChar == 'Y' || c.KeyChar == 'y')
                return true;
            return false;
        }

        // Ham xu ly in thong bao cac loi
        static void ThongBaoLoi(string st)
        {
            Console.Write(st);
            Console.ReadKey();
        }

        // Ham kiem tra danh sach rong
        //static bool LaRong(string mucdich)
        //{
        //    if (listFC.Count == 0)
        //    {
        //        Console.WriteLine("\nCo so du lieu khong co flashcard nao {0}.", mucdich);
        //        Console.ReadKey();
        //        return true;
        //    }
        //    return false;
        //}

        // xu ly nhap ma don dang ky muon phong
        //static int XulyNhapMa(string mucdich)
        //{
        //    int ma;
        //    Console.Write("Nhap ma don dang ky {0} : ", mucdich);
        //    while (!int.TryParse(Console.ReadLine(), out ma)
        //          || ma < 0)
        //    {
        //        Console.WriteLine("Ma don dang ky ban nhap khong dung.");
        //        if (XuLyCauHoiYesNo("Ban co muon nhap lai khong ? (Y/N) : "))
        //            Console.Write("Moi ban nhap lai :");
        //        else
        //            break;
        //    }
        //    return ma;
        //}

        // Ham tra ra vi tri cua mot flashcard theo tu
        //static int LayIndexTheoTu(string tu)
        //{
        //    for (int i = 0; i < listFC.Count; i++)
        //        if (tu == listFC[i].tu)
        //            return i;
        //    return -1;
        //}
        #endregion
    }
}

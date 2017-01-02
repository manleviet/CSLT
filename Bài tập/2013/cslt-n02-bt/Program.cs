using System;
using System.Collections.Generic;

namespace QLHS
{
    // Cau truc luu tru ngay thang nam
    struct NgayThang
    {
        public int ngay;
        public int thang;
        public int nam;
    }

    // Cau truc luu tru diem
    struct Diem
    {
        public float toan;
        public float ly;
        public float hoa;
        public float van;
        public float anhvan;
    }

    // Cau truc lu tru thong tin ho so
    struct HocSinh
    {
        public string hoten;
        public NgayThang ngaysinh;
        public string quequan;

        public Diem diem;
        public float dtb;
    }

    class Program
    {
        // khai bao bien listHS de luu tru danh sach cac ho so
        static List<HocSinh> listHS;

        static void Main(string[] args)
        {
            // khoi tao listHS
            listHS = new List<HocSinh>();

            // xu ly menu
            int menu;
            while (true) 
            {
                InMenu(); // in menu ra man hinh
                menu = XulyChonMenu(1, 7); // lay gia tri nguoi su dung nhap

                switch (menu)
                {
                    case 1: // Nhap ho so moi
                        ThemHSMoi();
                        break;
                    case 2: // Sua thong tin ho so
                        SuaHS();
                        break;
                    case 3: // Xoa ho so
                        XoaHS();
                        break;
                    case 4: // Sua diem
                        SuaDiem();
                        break;
                    case 5: // In Danh sach trung tuyen
                        InDSTrungTuyen();
                        break;
                    case 6: // In Tat ca
                        InTatCaHS();
                        break;
                    case 7:
                        if (XuLyCauHoiYesNo("Ban co chac muon thoat chuong trinh ? (Y/N):"))
                        {
                            Console.WriteLine("\nCam on ban da su dung chuong trinh !");
                            Console.ReadKey();
                            Environment.Exit(0);
                        }
                        break;
                }
            }
        }

        #region Xu ly menu
        // In menu cua chuong trinh
        static void InMenu()
        {
            Console.WriteLine();
            Console.Write("CHUONG TRINH XET TUYEN SINH VIEN");
            Console.WriteLine();
            Console.WriteLine("1 - Them ho so moi");
            Console.WriteLine("2 - Sua thong tin ho so");
            Console.WriteLine("3 - Xoa ho so");
            Console.WriteLine("4 - Sua diem");
            Console.WriteLine("5 - In danh sach trung tuyen");
            Console.WriteLine("6 - In tat ca hoc sinh");
            Console.WriteLine("7 - Thoat");
            Console.WriteLine("Hay chon so tu 1 den 7 phu hop voi chuc nang tuong ung.");
            Console.Write("Ban chon so :");
        }

        // ham nay tra ra mot so tu 1 den 7 tuong ung voi cac so tren menu
        // ma nguoi su dung nhap
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
                Console.Write("Moi ban chon lai :");
            }
            return menu;
        }
        #endregion

        #region Nhap Ho So
        // Ham nay xu ly them ho so moi
        // viec them ho so moi chi dung lai khi nguoi su dung khong muon nhap tiep
        static void ThemHSMoi()
        {
            do
            {
                HocSinh hs = ThemMotHSMoi(); // them ho so moi
                listHS.Add(hs);
            } while (XuLyCauHoiYesNo("\nBan co muon nhap them ho so khac khong ? (Y/N) :"));
        }

        // Ham nay tra ra mot ho so xet tuyen moi thong qua mot bien kieu cau truc HocSinh
        static HocSinh ThemMotHSMoi()
        {
            HocSinh hs = new HocSinh(); // khoi tao mot bien cau truc HocSinh
            Console.WriteLine("\nMoi ban nhap thong tin hoc sinh :");
            Console.Write("Ho va ten :");
            hs.hoten = XulyNhapTen(); // nhap ho ten
            hs.ngaysinh = XulyNhapNgaySinh(); // nhap ngay sinh
            Console.Write("Que quan :");
            hs.quequan = Console.ReadLine(); // nhap que quan
            hs.diem.toan = XulyNhapDiem("Diem Toan :"); // nhap diem toan
            hs.diem.ly = XulyNhapDiem("Diem Ly :"); // nhap diem ly
            hs.diem.hoa = XulyNhapDiem("Diem Hoa :"); // nhap diem hoa
            hs.diem.van = XulyNhapDiem("Diem Van :"); // nhap diem van
            hs.diem.anhvan = XulyNhapDiem("Diem Anh Van :"); // nhap diem anh van

            return hs;
        }

        // Xu ly cho viec nhap ho ten
        static string XulyNhapTen()
        {
            string hoten = Console.ReadLine();
            while (hoten.Equals("") || LayIndexTheoTen(hoten) != -1)
                // neu chuoi ho ten la rong hoac hoten do da co trong danh sach ho so
                // thi yeu cau nhap lai
            {
                Console.WriteLine("Ho ten khong duoc rong.");
                Console.Write("Moi ban nhap lai :");
                hoten = Console.ReadLine();
            }
            return hoten;
        }

        // Xu ly cho viec nhap cac loai diem
        static float XulyNhapDiem(string thongbao)
        {
            float diem;
            Console.Write(thongbao);
            while (!float.TryParse(Console.ReadLine(), out diem)
                   || diem < 0
                   || diem > 10) // neu khong phai la so hoac so khong thuoc doan [0, 10]
                                 // thi yeu cau nhap lai
            {
                Console.WriteLine("Diem ban nhap bi sai.");
                Console.Write("De nghi ban nhap lai :");
            }
            return diem;
        }

        // Xu ly nhap ngay thang nam sinh
        static NgayThang XulyNhapNgaySinh()
        {
            NgayThang nt = new NgayThang();
            int ngay, thang, nam;

            Console.Write("Ngay sinh (mm/dd/yyyy) :");
            string ngaysinh = Console.ReadLine(); // Nhap chuoi ngay sinh theo dang dd/mm/yyyy
            string[] items = ngaysinh.Split('/'); // tach chuoi ngay sinh de lay ra cac so ngay, thang, nam

            while (items.Length != 3 ||
                   !int.TryParse(items[0], out ngay) ||
                   !int.TryParse(items[1], out thang) ||
                   !int.TryParse(items[2], out nam) ||
                   !LaNgayHopLe(ngay, thang, nam)) 
                   // neu ngay, thang, nam khong phai la so hoac
                   // ngay thang nam do khong hop le (Vd: ngay 29 cua thang 2 nhung nam khong nhuan)
                   // thi yeu cau nhap lai
            {
                Console.WriteLine("Ngay ban nhap khong dung.");
                Console.Write("De nghi ban nhap lai theo dang mm/dd/yyyy :");
                ngaysinh = Console.ReadLine();
                items = ngaysinh.Split('/');
            }

            nt.ngay = ngay;
            nt.thang = thang;
            nt.nam = nam;

            return nt;
        }
        #endregion

        #region Sua Ho So
        // Xu ly sua thong tin ho so
        static void SuaHS()
        {
            // neu danh sach chua co ho so nao thi khong lam gi ca
            if (LaRong())
                return;

            int indexHS;
            string hoten;
            Console.WriteLine("\nBan sua thong tin cho ho so nao ?");
            do
            {
                Console.Write("Nhap ho ten hoc sinh :");
                hoten = Console.ReadLine(); // Nhap ten ho so can sua

                if ((indexHS = LayIndexTheoTen(hoten)) == -1) // Lay vi tri cua ho so trong danh sach
                    // neu khong co thi thuc hien lan lap tiep theo
                {
                    Console.WriteLine("Ten hoc sinh ban nhap khong co trong du lieu.");
                    continue;
                }

                Console.WriteLine("Ho so cua hoc sinh {0} co thong tin nhu sau :", listHS[indexHS].hoten);
                InMotHS(listHS[indexHS]); // in thong tin hien tai cua ho so

                Console.WriteLine("\nMoi ban nhap thong tin moi cho ho so tren.");
                SuaMotHS(indexHS); // goi ham sua thong tin ho so

            } while (XuLyCauHoiYesNo("\nBan co tiep tuc sua thong tin cho hoc sinh khac? (Y/N):"));
        }

        static void SuaMotHS(int index)
        {
            HocSinh hs = listHS[index]; // lay thong tin ho so ra de sua

            // nhap thong tin moi vao
            Console.Write("Ho va ten :");
            hs.hoten = XulyNhapTen();
            hs.ngaysinh = XulyNhapNgaySinh();
            Console.Write("Que quan :");
            hs.quequan = Console.ReadLine();
            hs.diem.toan = XulyNhapDiem("Diem Toan :");
            hs.diem.ly = XulyNhapDiem("Diem Ly :");
            hs.diem.hoa = XulyNhapDiem("Diem Hoa :");
            hs.diem.van = XulyNhapDiem("Diem Van :");
            hs.diem.anhvan = XulyNhapDiem("Diem Anh Van :");

            listHS[index] = hs; // gan cau truc moi vao danh sach tai vi tri ho so muon sua
        }
        #endregion

        #region Xoa Ho So
        // Xu ly xoa ho so
        static void XoaHS()
        {
            if (LaRong()) return; // neu danh sach rong thi khong lam gi ca

            int indexHS;
            string hoten;
            Console.WriteLine("\nBan can xoa ho so nao ?");
            do
            {
                Console.Write("Nhap ho ten hoc sinh trong ho so :");
                hoten = Console.ReadLine(); // nhap ho ten ho so

                if ((indexHS = LayIndexTheoTen(hoten)) == -1) // neu khong co ho so voi ho ten do
                    Console.WriteLine("Ten hoc sinh ban nhap khong co trong du lieu.");
                else // neu co
                {
                    Console.WriteLine("Ho so cua hoc sinh {0} co thong tin nhu sau :", listHS[indexHS].hoten);
                    InMotHS(listHS[indexHS]); // In thong tin ho so

                    // Hoi co that su muon xoa hay khong
                    if (XuLyCauHoiYesNo("\nBan co that su muon xoa ho so nay ? (Y/N):"))
                    { // neu co
                        listHS.RemoveAt(indexHS); // xoa khoi danh sach
                        Console.WriteLine("Ho so cua hoc sinh {0} da duoc xoa.", hoten);
                        Console.ReadKey();
                    }
                }
            } while (XuLyCauHoiYesNo("\nBan co tiep tuc xoa nhung ho so khac? (Y/N):"));
        }
        #endregion

        #region Sua Diem
        // Xu ly sua diem
        static void SuaDiem()
        {
            if (LaRong()) return; // khong lam gi khi danh sach khong co ho so nao

            int indexHS;
            string hoten;
            Console.WriteLine("\nBan muon sua diem cho ho so nao ?");
            do
            {
                Console.Write("Nhap ho ten hoc sinh :");
                hoten = Console.ReadLine(); // Nhap ho ten ho so muon sua diem

                if ((indexHS = LayIndexTheoTen(hoten)) == -1) // neu khong co ho so muon sua diem
                    Console.WriteLine("Ten hoc sinh ban nhap khong co trong du lieu.");
                else // neu co
                {
                    Console.WriteLine("Ho so cua hoc sinh {0} co thong tin nhu sau :", listHS[indexHS].hoten);
                    InMotHS(listHS[indexHS]); // in thong tin ho so muon sua diem

                    SuaDiemMotHS(indexHS); // thuc hien sua diem
                }
            } while (XuLyCauHoiYesNo("\nBan co tiep tuc sua diem cho hoc sinh khac? (Y/N):"));
        }

        // Sua diem cua mot ho so
        static void SuaDiemMotHS(int index)
        {
            HocSinh hs = listHS[index]; // lay ho so ra mot bien cau truc

            bool thoat = true;
            int menu;
            while (thoat)
            {
                InMenuSuaDiem(); // in menu cua chuc nang sua diem
                menu = XulyChonMenu(1, 6); // lay so nguoi su dung nhap

                switch (menu)
                {
                    case 1: // Sua diem Toan
                        hs.diem.toan = XulyNhapDiem("Diem Toan :");
                        break;
                    case 2: // Sua diem Ly
                        hs.diem.ly = XulyNhapDiem("Diem Ly :");
                        break;
                    case 3: // Sua diem Hoa
                        hs.diem.hoa = XulyNhapDiem("Diem Hoa :");
                        break;
                    case 4: // Sua diem Van
                        hs.diem.van = XulyNhapDiem("Diem Van :");
                        break;
                    case 5: // Sua diem Anh Van
                        hs.diem.anhvan = XulyNhapDiem("Diem Anh Van :");
                        break;
                    case 6: // Thoat
                        thoat = false;
                        break;
                }
            }

            listHS[index] = hs; // gan lai
        }

        // In menu cho chuc nang sua diem
        static void InMenuSuaDiem()
        {
            Console.WriteLine();
            Console.Write("Ban muon sua diem mon nao ?");
            Console.WriteLine();
            Console.WriteLine("1 - Toan");
            Console.WriteLine("2 - Ly");
            Console.WriteLine("3 - Hoa");
            Console.WriteLine("4 - Van");
            Console.WriteLine("5 - Anh Van");
            Console.WriteLine("6 - Khong sua nua");
            Console.WriteLine("Hay chon so tu 1 den 6 phu hop voi chuc nang tuong ung.");
            Console.Write("Ban chon so :");
        }
        #endregion

        #region In Danh sach trung tuyen
        // In danh sach trung tuyen
        static void InDSTrungTuyen()
        {
            // nhap diem san
            float diemsan = XulyNhapDiem("Moi nhap diem san :");

            // dem so ho so trung tuyen
            int count = 0;
            for (int i = 0; i < listHS.Count; i++)
            {
                TinhDiemTB(i);
                if (listHS[i].dtb >= diemsan) count++;
            }

            // in danh sach trung tuyen
            Console.WriteLine("\nCo {0} ho so trung tuyen.", count);
            count = 0;
            foreach (HocSinh hs in listHS)
            {
                if (hs.dtb < diemsan) continue;
                count++;
                Console.WriteLine("{0}.", count); // in so thu tu
                InMotHS(hs);
            }
            Console.ReadKey();
        }

        // Tinh diem trung binh
        static void TinhDiemTB(int index)
        {
            HocSinh hs = listHS[index];
            float tong = hs.diem.toan + 
                         hs.diem.ly + 
                         hs.diem.hoa + 
                         hs.diem.van + 
                         hs.diem.anhvan;
            hs.dtb = tong / 5;
            listHS[index] = hs;
        }
        #endregion

        #region In Tat Ca
        // In tat ca ho so trong danh sach listHS
        static void InTatCaHS()
        {
            Console.WriteLine("\nCo so du lieu co tat ca {0} ho so.", listHS.Count);
            for (int i = 0; i < listHS.Count; i++)
            {
                Console.WriteLine("{0}.", i + 1);
                InMotHS(listHS[i]);
            }
            Console.ReadKey();
        }

        // In thong tin cua mot ho so
        static void InMotHS(HocSinh hs)
        {
            Console.WriteLine("Ho ten: {0}", hs.hoten);
            Console.WriteLine("Ngay sinh: {0:d2}/{1:d2}/{2:d4}", 
                              hs.ngaysinh.ngay, 
                              hs.ngaysinh.thang, 
                              hs.ngaysinh.nam);
            Console.WriteLine("Que quan: {0}", hs.quequan);
            Console.WriteLine("---------");
            Console.WriteLine("Toan:\t{0:f1}", hs.diem.toan);
            Console.WriteLine("Ly:\t{0:f1}", hs.diem.ly);
            Console.WriteLine("Hoa:\t{0:f1}", hs.diem.hoa);
            Console.WriteLine("Van:\t{0:f1}", hs.diem.van);
            Console.WriteLine("Anh Van:\t{0:f1}", hs.diem.anhvan);
        }
        #endregion

        #region Cac ham dung chung
        // Xu ly cho cau hoi yes no
        // tham so cauhoi chua cau hoi muon hoi nguoi su dung
        // neu nguoi su dung chon Y hoac y thi ham tra ra true
        // nguoc lai, tra ra false
        static bool XuLyCauHoiYesNo(string cauhoi)
        {
            Console.Write(cauhoi);
            ConsoleKeyInfo c = Console.ReadKey();
            Console.WriteLine();

            if (c.KeyChar == 'Y' || c.KeyChar == 'y')
                return true;
            return false;
        }

        // Ham kiem tra xem listHS co rong hay khong
        // neu rong tra ra true, nguoc lai tra ra false
        static bool LaRong()
        {
            if (listHS.Count == 0)
            {
                Console.WriteLine("\nKhong co ho so nao trong du lieu.");
                Console.ReadKey();
                return true;
            }
            return false;
        }

        // Ham kiem tra xem ngay thang nam co hop le khong
        // tra ra true neu hop le
        static bool LaNgayHopLe(int ngay, int thang, int nam)
        {
            if (nam > 0 && thang > 0 && thang <= 12 &&
                ngay > 0 && ngay <= NgayCuoiCungCuaThang(thang, nam))
                return true;
            return false;
        }

        // Tra ve true neu nam la nam nhuan
        static bool LaNamNhuan(int nam)
        {
            return (((nam % 4) == 0) && ((nam % 100) != 0))
                    || ((nam % 400) == 0);
        }

        // Tra ve so ngay cua mot thang cho truoc
        static int NgayCuoiCungCuaThang(int thang, int nam)
        {
            switch (thang)
            {
                case 2:
                    if (LaNamNhuan(nam))
                        return 29;
                    else
                        return 28;
                case 4:
                case 6:
                case 9:
                case 11: return 30;
                default: return 31;
            }
        }

        // Ham kiem tra xem hai ho ten co giong nhau khong
        static bool TenGiongNhau(string hoten, HocSinh hs)
        {
            if (hoten.Equals(hs.hoten))
                return true;
            return false;
        }

        // Ham tra ra vi tri cua mot ho so nao do dua vao ho ten cua ho so do
        static int LayIndexTheoTen(string hoten)
        {
            for (int i = 0; i < listHS.Count; i++)
                if (TenGiongNhau(hoten, listHS[i]))
                    return i;
            return -1;
        }
        #endregion
    }
}

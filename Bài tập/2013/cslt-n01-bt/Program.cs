using System;
using System.Collections.Generic;
using System.IO;

namespace QLTC
{
    // Kieu enum de phan loai thu chi
    enum LoaiThuChi{Thu, Chi};
    enum PhanLoaiThu{Luong, Thuong, Lai, Khac};
    enum PhanLoaiChi{TienDien, TienNuoc, AoQuan, LuongThuc, YTe, Khac};

    // cau truc luu tru ngay thang nam
    struct NgayThang
    {
        public int ngay;
        public int thang;
        public int nam;
    }

    // cau truc luu tru mot khoan thu chi
    struct ThuChi
    {
        public int ma;
        public double sotien;
        public LoaiThuChi loaiTC;
        public List<PhanLoaiThu> loaiThu;
        public List<PhanLoaiChi> loaiChi;
        public NgayThang ngaythang;
    }

    // Kieu cu phap the hien cu phap dong lenh khi goi chuong trinh
    // Dang 1 : XuLySoLieu.exe
    // Dang 2 : XuLySoLieu.exe -h
    // Dang 3 : XuLySoLieu.exe <num>
    // Dang 4 : XuLySoLieu.exe <path>
    enum KieuCuPhap { Dang1, Dang2, Dang3, Dang4 };
    
    class Program
    {
        // bien listTC de luu tru danh sach cac khoan thu chi
        static List<ThuChi> listTC;
        // bien kieuCP de nhan dang cu phap dong lenh
        static KieuCuPhap kieuCP;

        static void Main(string[] args)
        {
            // khoi tao listTC
            listTC = new List<ThuChi>();

            // Kiem tra doi so dong lenh
            // de xac dinh dang cu phap dong lenh
            if (args.Length == 0)
                kieuCP = KieuCuPhap.Dang1; // khi khong co doi so nao thi thuc hien theo dang 1
            else if (args.Length == 1)
            { // khi co 1 doi so, co the thuoc vao dang 2, 3, 4
                int num;
                if (int.TryParse(args[0], out num)) // neu doi so do kieu int
                    kieuCP = KieuCuPhap.Dang3; // thi thuoc dang 3
                else if (args[0] == "-h") // neu la chuoi "-h"
                    kieuCP = KieuCuPhap.Dang2; // thuoc dang 2
                else // nguoc lai, thuoc dang 4
                    kieuCP = KieuCuPhap.Dang4;
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
                case KieuCuPhap.Dang1:
                    // Hien thi Menu
                    XuLyMenu();
                    break;
                case KieuCuPhap.Dang2:
                    InHuongDanSuDung(); // In ra huong dan su dung
                    Console.ReadKey(); // Cho nguoi sd doc huong dan va bam enter
                    Environment.Exit(0); // roi thoat khoi chuong trinh
                    break;
                case KieuCuPhap.Dang3:
                    // Goi ham nhap du lieu voi so luong phan tu da biet
                    ThemMucTCMoi(int.Parse(args[0]));
                    // Hien thi Menu
                    XuLyMenu();
                    break;
                case KieuCuPhap.Dang4:
                    // Doc du lieu tu file
                    DocFile(args[0]);
                    // Hien thi Menu
                    XuLyMenu();
                    break;
            }
        }

        // Ham thuc hien in ra huong dan su dung cho chuong trinh
        static void InHuongDanSuDung()
        {
            Console.WriteLine();
            Console.WriteLine("CHUONG TRINH QUAN LY THU CHI");
            Console.WriteLine("Cu phap cau lenh theo 1 trong 4 dang sau :");
            Console.WriteLine("XuLySoLieu.exe"); // Dang 1
            Console.WriteLine("XuLySoLieu.exe -h"); // Dang 2
            Console.WriteLine("XuLySoLieu.exe <num>"); // Dang 3
            Console.WriteLine("XuLySoLieu.exe <path>"); // Dang 4
            Console.WriteLine();
        }

        #region Xu ly menu
        // In menu cua chuong trinh
        static void InMenu()
        {
            Console.WriteLine();
            Console.Write("CHUONG TRINH QUAN LY THU CHI");
            Console.WriteLine();
            Console.WriteLine("1 - Them khoan thu chi moi");
            Console.WriteLine("2 - In thu chi theo thang nam");
            Console.WriteLine("3 - Xoa khoan thu chi");
            Console.WriteLine("4 - Sua thong tin khoan thu chi");
            Console.WriteLine("5 - Thong ke thu chi theo thang ");
            Console.WriteLine("6 - Thong ke theo phan loai");
            Console.WriteLine("7 - Lay du lieu tu file");
            Console.WriteLine("8 - Luu du lieu ra file");
            Console.WriteLine("9 - In tat ca cac muc thu chi");
            Console.WriteLine("10 - Thoat");
            Console.WriteLine("Hay chon so tu 1 den 10 phu hop voi chuc nang tuong ung.");
            Console.Write("Ban chon so :");
        }

        // ham xu ly menu
        static void XuLyMenu()
        {
            int menu;
            while (true)
            {
                InMenu(); // in menu ra man hinh
                menu = XulyChonMenu(1, 10); // cho nguoi su dung chon menu

                switch (menu)
                {
                    case 1: // Them muc thu chi moi
                        ThemMucTCMoi();
                        break;
                    case 2: // In thu chi theo thang nam
                        InTheoThangNam();
                        break;
                    case 3: // Xoa muc thu chi
                        XoaMucTC();
                        break;
                    case 4: // Sua thong tin muc thu chi
                        SuaMucTC();
                        break;
                    case 5: // Thong ke thu chi theo thang
                        ThongKeTheoThang();
                        break;
                    case 6: // Thong ke theo phan loai va thang
                        ThongKeTheoPhanLoai();
                        break;
                    case 7: // doc file
                        DocFile();
                        break;
                    case 8: // ghi du lieu ra file
                        GhiFile();
                        break;
                    case 9: // in tat ca cac khoan thu chi
                        InTatCa();
                        break;
                    case 10: // thoat khoi chuong trinh
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

        // ham nay tra ra mot so tu 1 den 10
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
                Console.Write("Moi ban chon lai :");
            }
            return menu;
        }
        #endregion

        // XU LY THEM MUC THU CHI MOI
        #region Them muc thu chi moi
        // Ham cho phep nhap them n khoan thu chi moi
        // ham nhap voi so luong nhap xac dinh truoc
        static void ThemMucTCMoi(int n)
        {
            for (int i = 0; i < n; i++)
            {
                Console.WriteLine("{0}.", i + 1);
                ThuChi tc = ThemMotMucTCMoi(); // nhap cho tung khoan thu chi
                listTC.Add(tc); // add vao listTC
            }
        }

        // Ham cho phep nhap vao cac khoan thu chi moi
        // viec nhap chi dung lai khi nguoi su dung muon
        static void ThemMucTCMoi()
        {
            do
            {
                ThuChi tc = ThemMotMucTCMoi(); // nhap thong tin cho tung khoan thu chi
                listTC.Add(tc); // add vao listTC
                // hoi nguoi su dung co muon tiep tuc khong
            } while (XuLyCauHoiYesNo("\nBan co muon nhap them muc khac khong ? (Y/N) :"));
        }

        // Ham xu ly nhap thong tin cho tung khoan thu chi
        static ThuChi ThemMotMucTCMoi()
        {
            ThuChi tc = new ThuChi(); // khoi tao
            tc.loaiThu = new List<PhanLoaiThu>();
            tc.loaiChi = new List<PhanLoaiChi>();

            Console.WriteLine("\nMoi ban nhap thong tin thu chi :");
            tc.ma = LayMaCuoiCung() + 1; // lay ma lon nhat tang len mot
            tc.sotien = XulyNhapTien(); // xu ly nhap tien
            tc.loaiTC = XulyChonLoaiTC(); // xu ly nhap loai thu hay chi
            XulyNhapPhanLoaiTC(ref tc); // xu ly nhap cac phan loai
            tc.ngaythang = XulyNhapNgayThang(); // xu ly nhap ngay thang

            return tc;
        }

        // Ham xu ly nhap tien
        static double XulyNhapTien()
        {
            double sotien;
            Console.Write("So tien :");
            // kiem tra du lieu nhap vao phai la so va lon hon 0
            while (!double.TryParse(Console.ReadLine(), out sotien)
                   || sotien < 0)
            { // khong phai thi nhap lai
                Console.WriteLine("So tien ban nhap la sai (Dung nhap so am).");
                Console.Write("Moi ban nhap lai :");
            }
            return sotien;
        }

        // Ham xu ly nhap loai thu chi
        static LoaiThuChi XulyChonLoaiTC()
        {
            Console.Write("Thu hay Chi (Nhap dung chu Thu hoac Chi):");
            LoaiThuChi loaiTC;
            // ep kieu sang enum LoaiThuChi
            while (!Enum.TryParse(Console.ReadLine(), out loaiTC))
            { // neu khong duoc thi nhap sai, yeu cau nhap lai
                Console.WriteLine("Ban nhap sai. Moi ban nhap lai :");
            }
            return loaiTC;
        }

        // Ham xu ly nhap cac phan loai thu chi
        static void XulyNhapPhanLoaiTC(ref ThuChi tc)
        {
            // cho phep nguoi su dung bo qua khong nhap phan loai
            if (!XuLyCauHoiYesNo("Ban co nhap cac phan loai ? (Y/N):")) return;

            string chuoiphanloai;
            string[] pls;
            PhanLoaiThu plThu;
            PhanLoaiChi plChi;
            switch (tc.loaiTC)
            {
                case LoaiThuChi.Chi: // neu khoan thu chi la loai Chi
                    // in menu cac phan loai chi
                    InMenuPhanLoai("TienDien, TienNuoc, AoQuan, ThucAn, SucKhoe, Khac");
                    // chuoi phan loai chi duoc nhap vao
                    // cac phan loai cach nhau bang khoang trong
                    chuoiphanloai = Console.ReadLine(); 
                    pls = chuoiphanloai.Split(' '); // tach cac phan loai chi
                    foreach (string st in pls) // duyet qua cac phan loai chi tach duoc
                        if (Enum.TryParse(st, out plChi)) // neu no dung la phan loai chi
                            tc.loaiChi.Add(plChi); // add vao danh sach cac phan loai chi
                    break;
                case LoaiThuChi.Thu: // neu khoan thu chi la loai Thu
                    // in menu cac phan loai thu
                    InMenuPhanLoai("Luong, Thuong, Lai, Khac");
                    chuoiphanloai = Console.ReadLine(); // tuong tu phan chi
                    pls = chuoiphanloai.Split(' ');
                    foreach (string st in pls)
                        if (Enum.TryParse(st, out plThu))
                            tc.loaiThu.Add(plThu);
                    break;
            }
        }

        // Ham in menu cac phan loai
        static void InMenuPhanLoai(string phanloai)
        {
            Console.Write("He thong ho tro cac phan loai sau cho mot muc chi : ");
            Console.WriteLine(phanloai);
            Console.WriteLine("Ban co the nhap nhieu phan loai, cac phan loai cac nhau bang khoang trong.");
            Console.Write("Moi ban nhap :");
        }

        // Ham xu ly nhap ngay thang
        static NgayThang XulyNhapNgayThang()
        {
            NgayThang nt = new NgayThang();
            int ngay, thang, nam;

            Console.Write("Ngay thang (mm/dd/yyyy) :"); // Yeu cau nhap ngay thang dung dang
            string ngaythang = Console.ReadLine();
            string[] items = ngaythang.Split('/'); // tach ngay, thang, nam

            // kiem tra :
            // 1. du 3 thanh phan : ngay, thang, nam
            // 2. ngay, thang, nam la cac so
            // 3. ngay thang nam do hop le
            while (items.Length != 3 ||
                   !int.TryParse(items[0], out ngay) ||
                   !int.TryParse(items[1], out thang) ||
                   !int.TryParse(items[2], out nam) ||
                   !LaNgayHopLe(ngay, thang, nam))
            { // neu trai lai thi yeu cau nhap lai
                Console.WriteLine("Ngay ban nhap khong dung.");
                Console.Write("De nghi ban nhap lai theo dang mm/dd/yyyy :");
                ngaythang = Console.ReadLine();
                items = ngaythang.Split('/');
            }

            // neu dung thi gan du lieu vao
            nt.ngay = ngay;
            nt.thang = thang;
            nt.nam = nam;

            return nt;
        }
        #endregion

        #region In an
        // Ham xu ly in cac khoan thu chi theo thang nam cho truoc
        static void InTheoThangNam()
        {
            int thang, nam;
            // Nguoi su dung nhap vao thang nam muon xem cac khoan thu chi
            XulyNhapThangNam(out thang, out nam);

            foreach (ThuChi tc in listTC) // duyet qua tat ca cac khoan thu chi
                // khoan nao co thoi gian bang voi thang nam cua nguoi su dung
                if (LaTrungThangNam(thang, nam, tc))
                    InMotMucTC(tc); // thi in thong tin khoan thu chi do ra
            Console.ReadKey();
        }

        // Ham xu ly in thong tin cua tat ca cac khoan thu chi
        static void InTatCa()
        {
            Console.WriteLine("\nCo so du lieu co tat ca {0} muc thu chi.", listTC.Count);
            for (int i = 0; i < listTC.Count; i++)
            {
                Console.WriteLine("{0}.", i + 1);
                InMotMucTC(listTC[i]);
            }
            Console.ReadKey();
        }

        // Ham xu ly in thong tin mot khoan thu chi
        // ThuChi tc : la khoan thu chi can in
        static void InMotMucTC(ThuChi tc)
        {
            Console.WriteLine("Ma: {0}", tc.ma); // in ma khoan thu chi
            // du vao loai Thu hay Chi de in ra "Thu vao" hoac "Chi ra" va so tien
            Console.WriteLine("{0} so tien : {1}", 
                              (tc.loaiTC == LoaiThuChi.Thu)?"Thu vao":"Chi ra", // su dung toan tu dieu kien
                              tc.sotien);
            Console.Write("Cac phan loai : ");
            // in cac phan loai
            switch (tc.loaiTC)
            {
                case LoaiThuChi.Thu:
                    foreach (PhanLoaiThu plThu in tc.loaiThu)
                        Console.Write("{0} ", plThu);
                    break;
                case LoaiThuChi.Chi:
                    foreach (PhanLoaiChi plChi in tc.loaiChi)
                        Console.Write("{0} ", plChi);
                    break;
            }
            Console.WriteLine("\nNgay : {0:d2}/{1:d2}/{2:d4}",
                              tc.ngaythang.ngay,
                              tc.ngaythang.thang,
                              tc.ngaythang.nam);
        }

        // Ham xu ly nhap thang nam
        static void XulyNhapThangNam(out int thang, out int nam)
        {
            Console.Write("\nNhap thang : ");
            // thang phai la mot so va nam trong khoang (0, 12]
            while (!int.TryParse(Console.ReadLine(), out thang)
                   || thang <= 0
                   || thang > 12)
                Console.Write("Ban nhap sai, moi ban nhap lai : ");

            Console.Write("Nhap nam : ");
            // nam phai la so va lon hon 0
            while (!int.TryParse(Console.ReadLine(), out nam)
                   || nam <= 0)
                Console.Write("Ban nhap sai, moi ban nhap lai : ");
        }
        #endregion

        #region Xoa
        // Ham xu ly xoa khoan thu chi
        static void XoaMucTC()
        {
            // neu la rong thi bo qua
            if (LaRong()) return;

            int index;
            int ma;
            Console.WriteLine("\nBan can xoa ho so nao ?");
            do
            {
                // xu ly nhap ma khoan thu chi
                ma = XulyNhapMaTC("de xoa");

                if ((index = LayIndexTheoMa(ma)) == -1) // neu ma khoan thu chi khong ton tai trong danh sach
                    Console.WriteLine("Ma thu chi ban nhap khong co trong du lieu.");
                else // nguoc lai
                {
                    Console.WriteLine("Muc thu chi voi ma {0} co thong tin nhu sau :", listTC[index].ma);
                    // in thong tin hien co cua khoan thu chi muon xoa
                    InMotMucTC(listTC[index]);

                    // xac dinh lai y muon xoa khoan thu chi cua nguoi dung
                    if (XuLyCauHoiYesNo("\nBan co that su muon xoa muc thu chi nay ? (Y/N):"))
                    {
                        listTC.RemoveAt(index); // xoa khoan thu chi khoi danh sach
                        Console.WriteLine("Muc thu chi voi {0} da duoc xoa.", ma);
                        Console.ReadKey();
                    }
                }
            } while (XuLyCauHoiYesNo("\nBan co tiep tuc xoa nhung muc thu chi khac? (Y/N):"));
        }

        // xu ly nhap ma khoan thu chi
        static int XulyNhapMaTC(string mucdich)
        {
            int ma;
            Console.Write("Nhap ma thu chi {0} : ", mucdich);
            while (!int.TryParse(Console.ReadLine(), out ma)
                  || ma < 0)
            {
                Console.WriteLine("Ma thu chi ban nhap khong dung.");
                Console.Write("Moi ban nhap lai :");
            }
            return ma;
        }
        #endregion

        #region Sua Muc Thu Chi
        // Ham xu ly sua thong tin cac khoan thu chi
        static void SuaMucTC()
        {
            // neu danh sach rong thi bo qua
            if (LaRong()) return;

            int index;
            int ma;
            Console.WriteLine("\nBan muon sua thong tin cho muc thu chi nao ?");
            do
            {
                // nhap ma khoan thu chi muon sua
                ma = XulyNhapMaTC("de sua");

                if ((index = LayIndexTheoMa(ma)) == -1) // neu khong co
                    Console.WriteLine("Muc thu chi ban nhap khong co trong du lieu.");
                else // nguoc lai
                {
                    Console.WriteLine("Ho so cua muc thu chi voi ma {0} co thong tin nhu sau :", listTC[index].ma);
                    InMotMucTC(listTC[index]); // in lai thong tin hien tai cua khoan thu chi muon sua

                    Console.WriteLine("\nMoi ban nhap thong tin moi cho muc thu chi tren.");
                    SuaMotMucTC(index); // thuc hien nhap thong tin moi cho khoan thu chi
                }
            } while (XuLyCauHoiYesNo("\nBan co tiep tuc sua thong tin cho muc thu chi khac? (Y/N):"));
        }

        // Ham cho nhap thong tin moi vao cho khoan thu chi muon sua thong tin
        static void SuaMotMucTC(int index)
        {
            ThuChi tc = listTC[index]; // lay khoan thu chi muon sua ra bien tc

            tc.sotien = XulyNhapTien(); // nhap lai tien
            tc.loaiTC = XulyChonLoaiTC(); // nhap lai loai thu chi
            XulyNhapPhanLoaiTC(ref tc); // nhap lai cac phan loai
            tc.ngaythang = XulyNhapNgayThang(); // nhap lai ngay thang

            listTC[index] = tc; // gan vao lai vi tri cu
        }
        #endregion

        #region Thong ke thu chi theo thang nam
        // Ham xu ly in thong ke theo thang
        static void ThongKeTheoThang()
        {
            int thang, nam;
            // nhap vao thang, nam muon in thong ke
            XulyNhapThangNam(out thang, out nam);

            // so du cua cac thang truoc
            double soduthangtruoc = TinhTongThuCacThangTruoc(thang, nam) - TinhTongChiCacThangTruoc(thang, nam);
            double[] thus = new double[4]; // thu trong thang theo 4 phan loai
            double[] chis = new double[6]; // chi trong thang theo 6 phan loai
            double tongThu = TinhTongThu(thang, nam); // tong thu cua thang
            double tongChi = TinhTongChi(thang, nam); // tong chi cua thang

            for (int i = 0; i < 4; i++)
                thus[i] = TinhTongThuTheoPhanLoai(thang, nam, (PhanLoaiThu)i);
            for (int i = 0; i < 6; i++)
                chis[i] = TinhTongChiTheoPhanLoai(thang, nam, (PhanLoaiChi)i);

            double soduhientai = soduthangtruoc + tongThu - tongChi;

            Console.WriteLine("\nSo du thang truoc : {0}", soduthangtruoc);
            Console.WriteLine("\nThong ke thu chi thang {0} nam {1} :", thang, nam);
            Console.WriteLine("\nTong thu: ", tongThu);
            InTKTheoPhanLoaiThu(thus[(int)PhanLoaiThu.Luong], tongThu, PhanLoaiThu.Luong);
            InTKTheoPhanLoaiThu(thus[(int)PhanLoaiThu.Thuong], tongThu, PhanLoaiThu.Thuong);
            InTKTheoPhanLoaiThu(thus[(int)PhanLoaiThu.Lai], tongThu, PhanLoaiThu.Lai);
            InTKTheoPhanLoaiThu(thus[(int)PhanLoaiThu.Khac], tongThu, PhanLoaiThu.Khac);

            Console.WriteLine("\nTong chi: ", tongChi);
            InTKTheoPhanLoaiChi(chis[(int)PhanLoaiChi.TienDien], tongChi, PhanLoaiChi.TienDien);
            InTKTheoPhanLoaiChi(chis[(int)PhanLoaiChi.TienNuoc], tongChi, PhanLoaiChi.TienNuoc);
            InTKTheoPhanLoaiChi(chis[(int)PhanLoaiChi.AoQuan], tongChi, PhanLoaiChi.AoQuan);
            InTKTheoPhanLoaiChi(chis[(int)PhanLoaiChi.LuongThuc], tongChi, PhanLoaiChi.LuongThuc);
            InTKTheoPhanLoaiChi(chis[(int)PhanLoaiChi.YTe], tongChi, PhanLoaiChi.YTe);
            InTKTheoPhanLoaiChi(chis[(int)PhanLoaiChi.Khac], tongChi, PhanLoaiChi.Khac);

            Console.WriteLine("\nSo du hien tai : {0}", soduhientai);
            Console.ReadKey();
        }

        // Ham xu ly in thong ke theo phan loai
        static void ThongKeTheoPhanLoai()
        {
            int thang, nam;
            // nhap thang, nam muon thong ke
            XulyNhapThangNam(out thang, out nam);

            Console.Write("Ban muon xem theo phan loai nao sau day: ");
            Console.WriteLine("TienDien, TienNuoc, AoQuan, ThucAn, SucKhoe, KhacChi, Luong, Thuong, Lai, KhacThu.");
            Console.WriteLine("Moi ban nhap :");
            string phanloai = Console.ReadLine(); // nhap phan loai muon thong ke
            PhanLoaiThu plThu;
            PhanLoaiChi plChi;

            if (Enum.TryParse(phanloai, out plThu)) // neu phan loai cua phan loai Thu
            {
                double tong = TinhTongThu(thang, nam); // tinh tong thu
                double tongPL = TinhTongThuTheoPhanLoai(thang, nam, plThu); // tinh tong theo phan loai
                InTKTheoPhanLoaiThu(tongPL, tong, plThu); // in thong ke
            }
            else if (Enum.TryParse(phanloai, out plChi)) // neu phan loai cua phan loai Chi
            {
                double tong = TinhTongChi(thang, nam); // tinh tong chi
                double tongPL = TinhTongChiTheoPhanLoai(thang, nam, plChi); // tinh tong theo phan loai
                InTKTheoPhanLoaiChi(tongPL, tong, plChi); // in thong ke
            }
            else if (phanloai == "KhacChi") // truong hop phan loai Khac cua Chi
            {
                double tong = TinhTongChi(thang, nam);
                double tongPL = TinhTongChiTheoPhanLoai(thang, nam, PhanLoaiChi.Khac);
                InTKTheoPhanLoaiChi(tongPL, tong, PhanLoaiChi.Khac);
            }
            else if (phanloai == "KhacThu") // truong hop phan loai Khac cua Thu
            {
                double tong = TinhTongThu(thang, nam);
                double tongPL = TinhTongThuTheoPhanLoai(thang, nam, PhanLoaiThu.Khac);
                InTKTheoPhanLoaiThu(tongPL, tong, PhanLoaiThu.Khac);
            }
            else // nhap sai du lieu
            {
                Console.WriteLine("Phan loai ban nhap khong ho tro trong he thong.");
                Console.ReadKey();
                return;
            }
        }

        // Ham tinh tong thu theo phan loai Thu
        static double TinhTongThuTheoPhanLoai(int thang, int nam, PhanLoaiThu plThu)
        {
            double tong = 0;
            foreach (ThuChi tc in listTC)
                if (LaTrungThangNam(thang, nam, tc)
                    && tc.loaiThu.Contains(plThu))
                    tong += tc.sotien;
            return tong;
        }
        
        // Ham tinh tong chi theo phan loai Chi
        static double TinhTongChiTheoPhanLoai(int thang, int nam, PhanLoaiChi plChi)
        {
            double tong = 0;
            foreach (ThuChi tc in listTC)
                if (LaTrungThangNam(thang, nam, tc)
                    && tc.loaiChi.Contains(plChi))
                    tong += tc.sotien;
            return tong;
        }

        // Ham tinh tong thu
        static double TinhTongThu(int thang, int nam)
        {
            double tong = 0;
            foreach (ThuChi tc in listTC)
                if (LaTrungThangNam(thang, nam, tc)
                    && tc.loaiTC == LoaiThuChi.Thu)
                    tong += tc.sotien;
            return tong;
        }

        // Ham tinh tong chi
        static double TinhTongChi(int thang, int nam)
        {
            double tong = 0;
            foreach (ThuChi tc in listTC)
                if (LaTrungThangNam(thang, nam, tc)
                    && tc.loaiTC == LoaiThuChi.Chi)
                    tong += tc.sotien;
            return tong;
        }

        // Ham tinh tong thu cac thang truoc
        static double TinhTongThuCacThangTruoc(int thang, int nam)
        {
            double tong = 0;
            foreach (ThuChi tc in listTC)
                // kiem tra khoan thu chi co ngay thang nam
                // truoc ngay 1 thang, nam nguoi su dung muon thong ke
                if (LaNgayTruoc(tc, 1, thang, nam) && tc.loaiTC == LoaiThuChi.Thu)
                    tong += tc.sotien;
            return tong;
        }

        static double TinhTongChiCacThangTruoc(int thang, int nam)
        {
            double tong = 0;
            foreach (ThuChi tc in listTC)
                // kiem tra khoan thu chi co ngay thang nam
                // truoc ngay 1 thang, nam nguoi su dung muon thong ke
                if (LaNgayTruoc(tc, 1, thang, nam) && tc.loaiTC == LoaiThuChi.Chi)
                    tong += tc.sotien;
            return tong;
        }

        // Ham xu ly in thong ke theo phan loai thu
        static void InTKTheoPhanLoaiThu(double tong, double tongThu, PhanLoaiThu plThu)
        {
            string st = "";
            switch (plThu) // chu yeu la xu ly in phan loai Thu
            {
                case PhanLoaiThu.Luong:
                    st = "luong";
                    break;
                case PhanLoaiThu.Thuong:
                    st = "thuong";
                    break;
                case PhanLoaiThu.Lai:
                    st = "lai";
                    break;
                case PhanLoaiThu.Khac:
                    st = "khac";
                    break;
            }
            Console.WriteLine("Thu theo {2}: {0}, chiem {1}%",
                              tong,
                              tong / tongThu,
                              st);
        }

        // Ham xu ly in thong ke theo phan loai Chi
        static void InTKTheoPhanLoaiChi(double tong, double tongChi, PhanLoaiChi plChi)
        {
            string st = "";
            switch (plChi) // chu yeu la xu ly in phan loai Chi
            {
                case PhanLoaiChi.TienDien:
                    st = "tien dien";
                    break;
                case PhanLoaiChi.TienNuoc:
                    st = "tien nuoc";
                    break;
                case PhanLoaiChi.LuongThuc:
                    st = "luong thuc";
                    break;
                case PhanLoaiChi.AoQuan:
                    st = "ao quan";
                    break;
                case PhanLoaiChi.YTe:
                    st = "y te";
                    break;
                case PhanLoaiChi.Khac:
                    st = "khac";
                    break;
            }
            Console.WriteLine("Chi cho {2}: {0}, chiem {1}%",
                              tong,
                              tong / tongChi,
                              st);
        }
        #endregion

        #region Xu ly file
        // Ham xu ly doc file
        static void DocFile(string filename = null)
        {
            while (filename == null) // neu khong co ten file thi yeu cau nhap lai
            {
                Console.Write("\nNhap duong dan den file du lieu : ");
                filename = Console.ReadLine();
            }

            // mo file ra de doc
            StreamReader reader = new StreamReader(filename);
            if (reader == null) // neu mo bi loi
            {
                ThongBaoLoiFile("File ban nhap khong ton tai hoac duong dan sai.");
                return; // thi thoat khoi ham
            }

            // doan code nay chua kiem tra su toan ven cua file du lieu
            string tam = null;
            int size;
            if ((tam = reader.ReadLine()) == null ||
                !int.TryParse(tam, out size)) 
                // neu khong doc duoc dong dau tien hay dong dau tien khong phai so
            {
                ThongBaoLoiFile("File bi loi!", reader);
                return; // file bi loi khong doc nua
            }

            ThuChi tc;
            string sotien = null, loaiTC = null, plThu = null, plChi = null, ngaythang = null;
            for (int i = 0; i < size; i++)
            {
                // doc cac dong tiep theo cua mot khoan thu chi
                if ((sotien = reader.ReadLine()) == null
                  || (loaiTC = reader.ReadLine()) == null
                  || (plThu = reader.ReadLine()) == null
                  || (plChi = reader.ReadLine()) == null
                  || (ngaythang = reader.ReadLine()) == null)
                {
                    ThongBaoLoiFile("File bi loi!", reader);
                    return; // neu bi loi thi thoat khoi ham
                }

                // neu tot
                tc = new ThuChi();
                tc.loaiThu = new List<PhanLoaiThu>();
                tc.loaiChi = new List<PhanLoaiChi>();

                tc.ma = LayMaCuoiCung() + 1; // gan ma khoan thu chi tu dong
                // O DAY, khong lay ma thu chi luu xuong file
                // vi co the bi trung voi ma thu chi hien co trong danh sach

                // lay so tien
                if (!double.TryParse(sotien, out tc.sotien))
                {
                    ThongBaoLoiFile("File bi loi!", reader);
                    return;
                }
                // lay loai Thu, Chi
                if (!Enum.TryParse(loaiTC, out tc.loaiTC))
                {
                    ThongBaoLoiFile("File bi loi!", reader);
                    return;
                }
                // lay cac phan loai Thu
                string[] sts = plThu.Split(' ');
                PhanLoaiThu loaiThu;
                foreach (string st in sts)
                    if (!Enum.TryParse(st, out loaiThu))
                    {
                        ThongBaoLoiFile("File bi loi!", reader);
                        return;
                    }
                    else
                        tc.loaiThu.Add(loaiThu);
                // lay cac phan loai Chi
                sts = plChi.Split(' ');
                PhanLoaiChi loaiChi;
                foreach (string st in sts)
                    if (!Enum.TryParse(st, out loaiChi))
                    {
                        ThongBaoLoiFile("File bi loi!", reader);
                        return;
                    }
                    else
                        tc.loaiChi.Add(loaiChi);
                // xu ly ngay thang
                sts = ngaythang.Split('/');
                if (sts.Length != 3 ||
                    !int.TryParse(sts[0], out tc.ngaythang.ngay) ||
                    !int.TryParse(sts[1], out tc.ngaythang.thang) ||
                    !int.TryParse(sts[2], out tc.ngaythang.nam) ||
                    !LaNgayHopLe(tc.ngaythang.ngay, tc.ngaythang.thang, tc.ngaythang.nam))
                {
                    ThongBaoLoiFile("File bi loi!", reader);
                    return;
                }

                listTC.Add(tc);
            }

            // Dong tap tin
            reader.Close();
        }

        // Xu ly luu danh sach xuong file
        static void GhiFile(string filename = null)
        {
            while (filename == null) // neu khong co duong dan
            {
                Console.Write("\nNhap duong dan den luu file du lieu : ");
                filename = Console.ReadLine(); // yeu cau nhap lai
            }

            // mo file ra de ghi
            StreamWriter writer = new StreamWriter(filename);
            if (writer == null) // mo khong duoc
            {
                Console.WriteLine("Duong dan sai.");
                return; // error
            }

            // Ghi dữ liệu vào tập tin
            writer.WriteLine(listTC.Count);
            foreach (ThuChi tc in listTC)
            {
                writer.WriteLine(tc.sotien);
                writer.WriteLine(tc.loaiTC);
                writer.WriteLine(LayChuoiPhanLoai(tc.loaiThu));
                writer.WriteLine(LayChuoiPhanLoai(tc.loaiChi));
                writer.WriteLine("{0:d2}/{1:d2}/{2:d4}", 
                                 tc.ngaythang.ngay, 
                                 tc.ngaythang.thang, 
                                 tc.ngaythang.nam);
            }

            writer.Close();
        }

        // Xu ly tao ra chuoi phan loai Thu de luu xuong file
        static string LayChuoiPhanLoai(List<PhanLoaiThu> listThu)
        {
            string st = "";
            foreach (PhanLoaiThu plThu in listThu)
                st += string.Format("{0} ", plThu); // moi phan loai cach nhau bang khoang trong
            if (st != "") // do du khoang trong cuoi chuoi
                st = st.Remove(st.Length - 1); // nen thuc hien xoa no di
            return st;
        }

        // Xu ly tao ra chuoi phan loai Chi de luu xuong file
        static string LayChuoiPhanLoai(List<PhanLoaiChi> listChi)
        {
            string st = "";
            foreach (PhanLoaiChi plChi in listChi)
                st += string.Format("{0} ", plChi);
            if (st != "")
                st = st.Remove(st.Length - 1);
            return st;
        }

        // Ham xu ly in thong bao cac loai file
        static void ThongBaoLoiFile(string st, StreamReader reader = null)
        {
            Console.Write(st);
            Console.ReadKey();
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

        // Ham kiem tra danh sach rong
        static bool LaRong()
        {
            if (listTC.Count == 0)
            {
                Console.WriteLine("\nKhong co muc thu chi nao trong du lieu.");
                Console.ReadKey();
                return true;
            }
            return false;
        }

        // Ham kiem tra xem ngay thang nam co hop le khong
        // tra ra true neu hop le
        static bool LaNgayHopLe(int ngay, int thang, int nam)
        {
            if (nam > 0 && thang > 0 && thang < 13 &&
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

        // Ham kiem tra ton tai ma khoan thu chi nao do trong danh sach khong.
        static bool LaTrungSTT(int ma)
        {
            foreach (ThuChi tc in listTC)
                if (tc.ma == ma)
                    return true;
            return false;
        }

        // Ham tra ra gia tri ma khoan thu chi lon nhat
        static int LayMaCuoiCung()
        {
            int max = -1;
            foreach (ThuChi tc in listTC)
                if (tc.ma > max)
                    max = tc.ma;
            return max;
        }

        // Ham tra ra vi tri cua mot khoan thu chi theo ma thu chi
        static int LayIndexTheoMa(int ma)
        {
            for (int i = 0; i < listTC.Count; i++)
                if (ma == listTC[i].ma)
                    return i;
            return -1;
        }

        // Ham tra ra true neu thang va nam cua khoan thu chi bang voi thang nam nao do
        static bool LaTrungThangNam(int thang, int nam, ThuChi tc)
        {
            if (nam == tc.ngaythang.nam && thang == tc.ngaythang.thang)
                return true;
            return false;
        }

        // Ham tra ra truoc neu ngay thang nam cua khoan thu chi la nho hon ngay thang nam nao do
        // dua tren ngay tuyet doi
        static bool LaNgayTruoc(ThuChi tc, int ngay, int thang, int nam)
        {
            int ngaytuyetdoi1 = NgayTuyetDoi(tc.ngaythang.ngay, tc.ngaythang.thang, tc.ngaythang.nam);
            int ngaytuyetdoi2 = NgayTuyetDoi(ngay, thang, nam);
            if (ngaytuyetdoi1 < ngaytuyetdoi2)
                return true;
            return false;
        }

        // Tinh so ngay trong nam cho den
        // thang/ngay/nam nhap vao
        static int SoNgayTuDauNam(int ngay, int thang, int nam)
        {
            // gia su cac thang deu 31 ngay
            int songay = (thang - 1) * 31 + ngay;

            // hieu chinh cac thang sau thang hai
            if (thang > 2)
            {
                songay = songay - ((4 * thang + 23) / 10);
                if (LaNamNhuan(nam))
                    songay = songay + 1;
            }

            return songay;
        }

        // Ham tinh ngay tuyet doi
        static int NgayTuyetDoi(int ngay, int thang, int nam)
        {
            return SoNgayTuDauNam(ngay, thang, nam) // so ngay trong nam
                    + 365 * (nam - 1)          // so ngay cua cac nam truoc do
                    + (nam - 1) / 4   // nam nhuan Julian
                    - (nam - 1) / 100 // nam nhuan chan tram
                    + (nam - 1) / 400; // nam nhuan Gregorian
        }
        #endregion
    }
}

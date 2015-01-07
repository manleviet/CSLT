using System;
using System.Collections.Generic;
using System.IO;

namespace QLMPM
{
    #region Vung khai bao kieu du lieu
    // cau truc luu tru ngay thang nam
    struct NgayThang
    {
        public int ngay;
        public int thang;
        public int nam;
    }

    // cau truc luu tru mot don muon phong
    struct DonMuonPhong
    {
        public int ma; // ma don muon phong
        public string tenNgMuon; // ten nguoi muon phong
        public string phongMuon; // phong muon
        public NgayThang ngayMuon;
        public int tietBDMuon; // tiet bat dau muon
        public int tietKTMuon; // tiet ket thuc muon
    }

    // Kieu cu phap the hien cu phap dong lenh khi goi chuong trinh
    // Dang 1 : QLMPM.exe
    // Dang 2 : QLMPM.exe -h
    // Dang 3 : QLMPM.exe <num>
    // Dang 4 : QLMPM.exe <path>
    enum KieuCuPhap { Dang1, Dang2, Dang3, Dang4 };
    #endregion

    class Program
    {
        #region Khai bao bien
        // bien listDonMP de luu tru danh sach cac don muon phong
        static List<DonMuonPhong> listDonMP;
        // bien kieuCP de nhan dang cu phap dong lenh
        static KieuCuPhap kieuCP;
        #endregion

        #region Main
        static void Main(string[] args)
        {
            // khoi tao listDonMP;
            listDonMP = new List<DonMuonPhong>();

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
                    ThemDonDKMoi(int.Parse(args[0]));
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
            Console.WriteLine("CHUONG TRINH QUAN LY DON DANG KY MUON PHONG MAY TINH");
            Console.WriteLine("Cu phap cau lenh theo 1 trong 4 dang sau :");
            Console.WriteLine("QLMPM.exe"); // Dang 1
            Console.WriteLine("QLMPM.exe -h"); // Dang 2
            Console.WriteLine("QLMPM.exe <num>"); // Dang 3
            Console.WriteLine("QLMPM.exe <path>"); // Dang 4
            Console.WriteLine();
        }
        #endregion

        #region Xu ly menu
        // In menu cua chuong trinh
        static void InMenu()
        {
            Console.WriteLine();
            Console.Write("CHUONG TRINH QUAN LY DON DANG KY MUON PHONG MAY TINH");
            Console.WriteLine();
            Console.WriteLine("1 - Them don dang ky moi");
            Console.WriteLine("2 - Sua don dang ky muon phong");
            Console.WriteLine("3 - Xoa don dang ky muon phong");
            Console.WriteLine("4 - Danh sach tat ca cac don muon phong");
            Console.WriteLine("5 - Danh sach don muon phong theo thoi gian");
            Console.WriteLine("6 - Danh sach don muon phong theo nguoi muon va thang");
            Console.WriteLine("7 - Thong ke tan suat su dung");
            Console.WriteLine("8 - Doc du lieu tu file");
            Console.WriteLine("9 - Ghi du lieu ra file");
            Console.WriteLine("10 - Thoat");
            Console.WriteLine("Hay chon so tu 1 den 10 phu hop voi chuc nang tuong ung.");
            Console.Write("Ban chon so : ");
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
                        ThemDonDKMoi();
                        //ThemMucTCMoi();
                        break;
                    case 2: // Sua don muon phong
                        SuaDonDK();
                        break;
                    case 3: // Xoa don muon phong
                        XoaDonDK();
                        break;
                    case 4: // in tat ca cac don muon phong
                        InTatCa();
                        break;
                    case 5: // in danh sach cac don theo thoi gian
                        InTheoThoiGian();
                        break;
                    case 6: // in danh sach don muon phong theo nguoi muon va thang
                        InTheoNgMuonVaThang();
                        break;
                    case 7: // thong ke tan suat
                        ThongKeTanSuat();
                        break;
                    case 8: // doc file
                        DocFile();
                        break;
                    case 9: // luu file
                        GhiFile();
                        break;
                    case 10: // thoat khoi chuong trinh
                        if (XuLyCauHoiYesNo("Ban co chac muon thoat chuong trinh ? (Y/N): "))
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
                Console.Write("Moi ban chon lai : ");
            }
            return menu;
        }

        // Ham in menu cac kieu tai khoan, loai giao dich
        // chuoiLoai : la chuoi ky tu ten cac kieu tai khoan hoac loai giao dich
        static void InMenuCacLoai(string tieude, string chuoiLoai)
        {
            Console.Write("{0} sau : ", tieude);
            Console.WriteLine(chuoiLoai);
            Console.Write("Moi ban nhap : ");
        }
        #endregion

        // XU LY THEM DON DANG KY MOI
        #region Them don dang ky moi
        // Ham cho phep nhap them n don dang ky moi
        // ham nhap voi so luong nhap xac dinh truoc
        static void ThemDonDKMoi(int n)
        {
            Console.WriteLine("\nTHEM DON DANG KY MOI");

            for (int i = 0; i < n; i++)
            {
                Console.WriteLine("{0}.", i + 1);
                DonMuonPhong don = ThemMotDonDKMoi(); // nhap cho mot don dang ky
                if (don.ma != -1) // neu qua trinh nhap khong bi loi
                    ThemDonDKVaoDS(don); // them don vao danh sach
            }
        }

        // Ham cho phep nhap vao cac don dang ky moi
        // viec nhap chi dung lai khi nguoi su dung muon
        static void ThemDonDKMoi()
        {
            Console.WriteLine("\nTHEM DON DANG KY MOI");

            do
            {
                DonMuonPhong don = ThemMotDonDKMoi(); // nhap thong tin cho mot don dang ky
                // kiem tra xem ma don co bang -1 khong ?
                // neu khong thi chen vao vi tri thich hop de tao ra danh sach tang dan
                // theo ngay thang va tiet
                if (don.ma != -1)
                    ThemDonDKVaoDS(don);
                // hoi nguoi su dung co muon tiep tuc khong
            } while (XuLyCauHoiYesNo("\nBan co muon nhap them don khac khong ? (Y/N) : "));
        }

        // Ham chen mot don dang ky moi vao vi tri thich hop
        // voi danh sach tang dan theo ngay muon va tiet muon
        static void ThemDonDKVaoDS(DonMuonPhong don)
        {
            int vitri = LayViTriChenThichHop(don);
            if (vitri == -1)
                listDonMP.Add(don); // add vao listDonMP
            else
                listDonMP.Insert(vitri, don);
        }

        // Ham tra ra vi tri thich hop de chen don dang ky moi vao danh sach
        // sao cho danh sach tao ra se duoc sap xep tang dan theo ngay thang va tiet muon
        static int LayViTriChenThichHop(DonMuonPhong don)
        {
            for (int i = 0; i < listDonMP.Count; i++)
                if (LaNgayTruoc(don.ngayMuon, listDonMP[i].ngayMuon) // neu ngay thang nho hon
                    || (LaTrungNgayThangNam(don.ngayMuon, listDonMP[i].ngayMuon) // hoac neu ngay thang bang nhau
                    && don.tietBDMuon <= listDonMP[i].tietBDMuon)) // nhung tiet muon nho hon hoac bang
                    return i;
            return -1;
        }

        // Ham xu ly nhap thong tin cho tung khoan thu chi
        // Ham tra ra gia tri don voi ma -1 khi du lieu nhap vao bi trung
        static DonMuonPhong ThemMotDonDKMoi()
        {
            DonMuonPhong don = new DonMuonPhong(); // khoi tao

            Console.WriteLine("\nMoi ban nhap thong tin don dang ky muon phong :");
            don.ma = LayMaDonDKMoi();
            Console.WriteLine("Ma : {0}", don.ma);
            XuLyNhapTenPhongNgayTietMuon(ref don);

            return don;
        }

        // Ham lay ma don dang ky moi nhat
        static int LayMaDonDKMoi()
        {
            int max = 0;
            foreach (DonMuonPhong don in listDonMP)
                if (max < don.ma)
                    max = don.ma;
            return max + 1;
        }
        #endregion

        // Cac ham trong region Them don dang ky moi, Sua don dang ky va In an
        // su dung cac ham trong region nay
        #region Xu Ly Nhap Du Lieu
        // Ham xu ly viec nhap rieng thong tin ten, phong, ngay va tiet muon
        static void XuLyNhapTenPhongNgayTietMuon(ref DonMuonPhong don)
        {
            Console.Write("Ten nguoi muon : ");
            don.tenNgMuon = Console.ReadLine();
            don.phongMuon = XuLyChonPhong();
            don.ngayMuon = XuLyNhapNgayMuon();
            don.tietBDMuon = XuLyNhapTiet("bat dau", 0);
            don.tietKTMuon = XuLyNhapTiet("ket thuc", don.tietBDMuon);

            // kiem tra trung
            while (LaTrungNgayTietPhongMuon(don.ngayMuon, don.tietBDMuon, don.tietKTMuon, don.phongMuon))
            {   // hoi co muon sua lai khong ?
                Console.WriteLine("Ngay, tiet va phong muon ban chon da co nguoi dang ky roi!");
                if (XuLyCauHoiYesNo("Ban co muon sua lai ngay, tiet va phong muon khong? (Y/N) : "))
                {   // neu co thi cho sua lai
                    Console.WriteLine("Moi ban nhap lai:");
                    don.phongMuon = XuLyChonPhong();
                    don.ngayMuon = XuLyNhapNgayMuon();
                    don.tietBDMuon = XuLyNhapTiet("bat dau", 0);
                    don.tietKTMuon = XuLyNhapTiet("ket thuc", don.tietBDMuon);
                }
                else
                {   //neu khong thi sua lai ma = -1
                    don.ma = -1;
                    break;
                }
            }
        }

        // Ham xu ly chon phong muon
        static string XuLyChonPhong()
        {
            // in danh sach phong thuc hanh
            InMenuCacLoai("Nha truong co 4 phong thuc hanh", "A5.1, A5.2, B5.2, C4.1");
            string phong = Console.ReadLine();
            while ((phong != "A5.1") && (phong != "A5.2")
                && (phong != "B5.2") && (phong != "C4.1"))
            {
                Console.Write("Ban nhap khong dung, moi ban nhap lai : ");
                phong = Console.ReadLine();
            }
            return phong;
        }

        // Ham xu ly nhap tiet
        static int XuLyNhapTiet(string mess, int tietbd)
        {
            int tiet;
            Console.Write("Tiet {0} : ", mess);
            // tiet nhap vao phai nam trong doan tu 1-10 (1 ngay hoc co 10 tiet)
            // va phai lon hon tietbd
            while (!int.TryParse(Console.ReadLine(), out tiet)
                   || tiet <= tietbd || tiet < 1 || tiet > 10)
            { // khong phai thi nhap lai
                string thongbao = mess == "" ? "" : string.Format(" va phai lon hon {0}", tietbd);
                Console.WriteLine("Tiet ban nhap la sai (phai tu 1-10{0}).", thongbao);
                Console.Write("Moi ban nhap lai : ");
            }
            return tiet;
        }

        // Ham xu ly nhap ngay muon
        static NgayThang XuLyNhapNgayMuon()
        {
            NgayThang ngayMuon = new NgayThang();
            int ngay, thang, nam;

            Console.Write("Ngay thang (dd/mm/yyyy) : "); // Yeu cau nhap ngay thang dung dang
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
                Console.Write("De nghi ban nhap lai theo dang dd/mm/yyyy : ");
                ngaythang = Console.ReadLine();
                items = ngaythang.Split('/');
            }

            // neu dung thi gan du lieu vao
            ngayMuon.ngay = ngay;
            ngayMuon.thang = thang;
            ngayMuon.nam = nam;

            return ngayMuon;
        }

        // Ham xu ly nhap thang va nam
        static void XuLyNhapThangNam(out int thang, out int nam)
        {
            Console.Write("Chon thang : ");
            // neu gia tri nhap vao khong phai so hoac nho hon, bang 0 hoac lon hon 12 
            while (!int.TryParse(Console.ReadLine(), out thang)
                    || thang <= 0 || thang > 12)
                Console.Write("Ban nhap sai, moi ban nhap lai : "); // yeu cau nhap lai

            Console.Write("Chon nam : ");
            // neu gia tri nhap vao khong phai so hoac nho hon 0
            while (!int.TryParse(Console.ReadLine(), out nam)
                    || nam < 0)
                Console.Write("Ban nhap sai, moi ban nhap lai : "); // yeu cau nhap lai
        }
        #endregion

        #region Sua Don Dang Ky
        // Ham xu ly sua thong tin cac don dang ky
        static void SuaDonDK()
        {
            Console.WriteLine("\nSUA THONG TIN DON MUON PHONG");
            // neu danh sach rong thi bo qua
            if (LaRong("de sua thong tin"))
                return;

            int index;
            int ma;
            Console.WriteLine("\nBan muon sua thong tin cho don dang ky nao ?");
            do
            {
                // nhap ma khoan thu chi muon sua
                ma = XulyNhapMa("de sua");

                if ((index = LayIndexTheoMa(ma)) == -1) // neu khong co
                    Console.WriteLine("Don dang ky ban nhap khong co trong du lieu.");
                else // nguoc lai
                {
                    Console.WriteLine("Don dang ky voi ma {0} co thong tin nhu sau :", listDonMP[index].ma);
                    InMotDonMP(listDonMP[index]); // in lai thong tin hien tai cua don dang ky muon sua

                    Console.WriteLine("\nMoi ban nhap thong tin moi cho don dang ky tren.");
                    SuaMotDonMP(index); // thuc hien nhap thong tin moi cho don dang ky
                }
            } while (XuLyCauHoiYesNo("\nBan co tiep tuc sua thong tin cho don dang ky khac? (Y/N):"));
        }

        // Ham cho nhap thong tin moi vao cho khoan thu chi muon sua thong tin
        static void SuaMotDonMP(int vitri)
        {
            DonMuonPhong donCu = listDonMP[vitri]; // sao chep don dang ky muon sua ra
            listDonMP.RemoveAt(vitri); // xoa don dang ky cu

            // nhap thong tin moi vao bien donMoi
            // donCu de do du phong truong hop nhap thong tin moi bi sai, khong muon nhap tiep
            // thi khoi phuc lai thong tin cu tai vi tri cu (vitri)
            DonMuonPhong donMoi = donCu;
            XuLyNhapTenPhongNgayTietMuon(ref donMoi);

            if (donMoi.ma != -1)
                ThemDonDKVaoDS(donMoi);
            else // khoi phuc lai don cu
                ThemDonDKVaoDS(donCu);
        }
        #endregion

        #region Xoa
        // Ham xu ly xoa don dang ky
        static void XoaDonDK()
        {
            Console.WriteLine("\nXOA DON MUON PHONG");
            // neu la rong thi bo qua
            if (LaRong("de xoa")) return;

            int index;
            int ma;
            Console.WriteLine("\nBan can xoa don nao ?");
            do
            {
                // xu ly nhap ma khoan thu chi
                ma = XulyNhapMa("de xoa");

                if ((index = LayIndexTheoMa(ma)) == -1) // neu ma don khong ton tai trong danh sach
                    Console.WriteLine("Ma don ban nhap khong co trong co so du lieu.");
                else // nguoc lai
                {
                    Console.WriteLine("Don dang ky voi ma {0} co thong tin nhu sau :", listDonMP[index].ma);
                    // in thong tin hien co cua don dang ky muon xoa
                    InMotDonMP(listDonMP[index]);

                    // xac dinh lai y muon xoa don dang ky cua nguoi dung
                    if (XuLyCauHoiYesNo("\nBan co that su muon xoa don nay ? (Y/N):"))
                    {
                        listDonMP.RemoveAt(index); // xoa don dang ky khoi danh sach
                        Console.WriteLine("Don dang ky voi {0} da duoc xoa.", ma);
                        Console.ReadKey();
                    }
                }
            } while (XuLyCauHoiYesNo("\nBan co tiep tuc xoa nhung muc thu chi khac? (Y/N):"));
        }
        #endregion

        #region In an
        // Ham xu ly in thong tin cua tat ca cac don muon phong
        static void InTatCa()
        {
            Console.WriteLine("\nIN TAT CA CAC DON MUON PHONG");

            Console.WriteLine("Co so du lieu co tat ca {0} don dang ky.", listDonMP.Count);
            for (int i = 0; i < listDonMP.Count; i++)
            {
                Console.WriteLine("{0}.", i + 1);
                InMotDonMP(listDonMP[i]);
                if ((i + 1) % 10 == 0) // cu in duoc 10 don la dung man hinh lai
                    Console.ReadKey(); // bam phim bat ky de in 10 don tiep theo
            }
            Console.ReadKey();
        }

        // Ham xu ly in danh sach cac don muon phong trong khoang thoi gian
        static void InTheoThoiGian()
        {
            Console.WriteLine("\nIN DS DON MUON PHONG THEO THOI GIAN");

            Console.WriteLine("Moi ban nhap ngay bat dau.");
            NgayThang ngayBD = XuLyNhapNgayMuon();
            Console.WriteLine("Moi ban nhap ngay ket thuc.");
            NgayThang ngayKT = XuLyNhapNgayMuon();

            Console.WriteLine("Danh sach cac don dang ky muon phong tu ngay {0:d2}/{1:d2}/{2:d4} den ngay {3:d2}/{4:d2}/{5:d4} :",
                               ngayBD.ngay, ngayBD.thang, ngayBD.nam, ngayKT.ngay, ngayKT.thang, ngayKT.nam);
            int i = 0;
            foreach (DonMuonPhong don in listDonMP)
            {
                // ngayBD <= don.ngayMuon <= ngayKT
                if ((LaNgayTruoc(ngayBD, don.ngayMuon) || LaTrungNgayThangNam(ngayBD, don.ngayMuon))
                    && (LaNgayTruoc(don.ngayMuon, ngayKT) || LaTrungNgayThangNam(ngayKT, don.ngayMuon)))
                {
                    Console.WriteLine("{0}.", ++i);
                    InMotDonMP(don);

                    if (i % 10 == 0) // in 10 don roi dung lai
                        Console.ReadKey(); // bam phim bat ky de in 10 don tiep theo
                }
            }

            if (i == 0) // neu khong co don nao nam trong khoang thoi gian tren thi in thong bao
                Console.WriteLine("Khong co don nao!");

            Console.ReadKey();
        }

        // Ham xu ly in danh sach cac don muon phong theo nguoi muon va thang
        static void InTheoNgMuonVaThang()
        {
            Console.WriteLine("\nIN DS DON MUON PHONG THEO NGUOI MUON VA THANG");

            Console.Write("Nhap ten nguoi muon : ");
            string ten = Console.ReadLine();
            int thang, nam;
            XuLyNhapThangNam(out thang, out nam);

            Console.WriteLine("");
            int i = 0;
            foreach (DonMuonPhong don in listDonMP)
                if (ten == don.tenNgMuon
                    && don.ngayMuon.thang == thang
                    && don.ngayMuon.nam == nam)
                {
                    Console.WriteLine("{0}.", ++i);
                    InMotDonMP(don);

                    if (i % 10 == 0) // in 10 don roi dung lai
                        Console.ReadKey(); // bam phim bat ky de in 10 don tiep theo
                }

            // neu khong co don nao thoa man thi in thong bao
            if (i == 0) Console.WriteLine("Khong co don nao!"); 
            Console.ReadKey();
        }

        // Ham thong ke tan suat su dung theo thang
        static void ThongKeTanSuat()
        {
            int[] solan = new int[4]; // su dung mang 4 thanh phan de luu so tiet su dung cua 1 phong
            // 4 phan tu tuong ung voi 4 phong A5.1, A5.2, B5.2 va C4.1

            Console.WriteLine("\nTHONG KE TAN SUAT.");
            // nhap thang can thong ke
            int thang, nam;
            XuLyNhapThangNam(out thang, out nam);

            // dem tan suat su dung trong 1 thang
            foreach(DonMuonPhong don in listDonMP)
                if (don.ngayMuon.thang == thang && don.ngayMuon.nam == nam)
                    switch (don.phongMuon)
                    {
                        case "A5.1":
                            solan[0] += don.tietKTMuon - don.tietBDMuon + 1;
                            break;
                        case "A5.2":
                            solan[1] += don.tietKTMuon - don.tietBDMuon + 1;
                            break;
                        case "B5.2":
                            solan[2] += don.tietKTMuon - don.tietBDMuon + 1;
                            break;
                        case "C4.1":
                            solan[3] += don.tietKTMuon - don.tietBDMuon + 1;
                            break;
                    }

            // so tiet toi da co the su dung phong may trong thang
            int sotiet = TongSoTietTrongThang(thang, nam); 
            Console.WriteLine("Thong ke thang {0} nam {1}:", thang, nam);
            Console.WriteLine("Phong A5.1 : {0} tiet, {1:f2}%", solan[0], (double)solan[0] / sotiet * 100);
            Console.WriteLine("Phong A5.2 : {0} tiet, {1:f2}%", solan[1], (double)solan[1] / sotiet * 100);
            Console.WriteLine("Phong B5.2 : {0} tiet, {1:f2}%", solan[2], (double)solan[2] / sotiet * 100);
            Console.WriteLine("Phong C4.1 : {0} tiet, {1:f2}%", solan[3], (double)solan[3] / sotiet * 100);
        }

        // Ham tinh tong so tiet co the trong thang
        // Dau vao : thang, nam can tinh so tiet
        static int TongSoTietTrongThang(int thang, int nam)
        {
            // tinh ngay tuyet doi cua ngay 01/thang/nam
            int ngayDauThang = NgayTuyetDoi(1, thang, nam);
            // xac dinh ngay cuoi cung cua thang la ngay nao (28, 29, 30 hay 31 ?)
            int ngayCuoiCungCuaThang = NgayCuoiCungCuaThang(thang, nam);
            // tinh ngay tuyet doi cua ngay cuoi cung cua thang do
            int ngayCuoiThang = NgayTuyetDoi(ngayCuoiCungCuaThang, thang, nam);

            // cong thuc ky dieu
            int soNgayCN = (ngayCuoiThang - ngayDauThang - (ngayCuoiThang % 7 + 1) + 8) / 7;

            // so tiet bang so ngay trong thang - so ngay chu nhat * 10 (moi ngay 10 tiet)
            int soTiet = (ngayCuoiCungCuaThang - soNgayCN) * 10;

            return soTiet;
        }

        // Ham xu ly in thong tin mot don muon phong
        // DonMuonPhong don : la don muon phong can in
        static void InMotDonMP(DonMuonPhong don)
        {
            Console.WriteLine("Ma : {0}", don.ma); // in ma don
            Console.WriteLine("Nguoi muon : {0}", don.tenNgMuon); // in ten nguoi muon
            Console.WriteLine("Phong muon : {0}", don.phongMuon); // in phong muon
            Console.WriteLine("Ngay muon : {0:d2}/{1:d2}/{2:d4}",
                              don.ngayMuon.ngay,
                              don.ngayMuon.thang,
                              don.ngayMuon.nam);
            Console.WriteLine("Tiet bat dau : {0}", don.tietBDMuon);
            Console.WriteLine("Tiet ket thuc : {0}", don.tietKTMuon);
        }
        #endregion

        #region Xu ly file
        // Ham xu ly doc file
        static void DocFile(string filename = null)
        {
            Console.WriteLine("\nDOC DU LIEU TU FILE");

            int soluongcu = listDonMP.Count;
            if (listDonMP.Count != 0)
            {
                Console.Write("\nChuc nang nay se chen them du lieu tu file vao CSDL.");
                if (!XuLyCauHoiYesNo("Ban co chac chan muon doc du lieu tu file ? (Y/N) : "))
                    return;
            }

            while (filename == null) // neu khong co ten file thi yeu cau nhap lai
            {
                Console.Write("\nNhap duong dan den tap tin chua du lieu : ");
                filename = Console.ReadLine();
            }

            // mo file ra de doc
            StreamReader reader = new StreamReader(filename);
            if (reader == null) // neu mo bi loi
            {
                ThongBaoLoiFile("File ban nhap khong ton tai hoac duong dan sai.");
                return; // thi thoat khoi ham
            }

            string dong = null;
            // doc mot dong trong file
            while ((dong = reader.ReadLine()) != null) // neu het file thi khong doc nua
            {
                DonMuonPhong don;
                // tach cac thanh phan trong dong doc duoc
                // va gan gia tri vao bien cau truc don
                if (!TachGiaTri(dong, out don))
                { // neu qua trinh tach bi loi thi bao loi
                    Console.WriteLine("File bi loi!", reader);
                    return; // thoat khoi ham Doc file
                }

                ThemDonDKVaoDS(don); // them don vao danh sach
            }

            Console.WriteLine("Da them duoc {0} don vao CSDL", listDonMP.Count - soluongcu);

            // Dong tap tin
            reader.Close();
        }

        // Ham co nhiem vu tach chuoi doc duoc tu file
        // gan gia tri vao bien don
        static bool TachGiaTri(string input, out DonMuonPhong don)
        {
            don = new DonMuonPhong();
            string[] items = input.Split(',');

            // phai chua 5 thanh phan
            if (items.Length != 5) return false;

            // tach ma
            if (!int.TryParse(items[0], out don.ma)) return false;

            // xu ly ngay thang
            string[] ntn = items[1].Split('/');
            if (ntn.Length != 3 ||
                !int.TryParse(ntn[0], out don.ngayMuon.ngay) ||
                !int.TryParse(ntn[1], out don.ngayMuon.thang) ||
                !int.TryParse(ntn[2], out don.ngayMuon.nam) ||
                !LaNgayHopLe(don.ngayMuon.ngay, don.ngayMuon.thang, don.ngayMuon.nam))
                return false;

            // xu ly tiet
            ntn = items[2].Split('-');
            if (ntn.Length != 2 ||
                !int.TryParse(ntn[0], out don.tietBDMuon) ||
                !int.TryParse(ntn[1], out don.tietKTMuon) ||
                don.tietBDMuon >= don.tietKTMuon || don.tietBDMuon < 0 || don.tietKTMuon > 10)
                return false;

            // ten nguoi muon va phong muon
            don.tenNgMuon = items[3];
            don.phongMuon = items[4];
            return true;
        }

        // Xu ly luu danh sach xuong file
        static void GhiFile(string filename = null)
        {
            Console.WriteLine("\nGHI DU LIEU RA FILE");

            while (filename == null) // neu khong co duong dan
            {
                Console.Write("\nNhap duong dan den luu file du lieu : ");
                filename = Console.ReadLine(); // yeu cau nhap lai
            }

            Console.Write("\nDang ghi du lieu ra file.");

            // mo file ra de ghi
            StreamWriter writer = new StreamWriter(filename);
            if (writer == null) // mo khong duoc
            {
                Console.WriteLine("\nDuong dan sai.");
                return; // error
            }

            Console.Write(".");

            // Ghi dữ liệu vào tập tin
            foreach (DonMuonPhong don in listDonMP)
            {
                writer.WriteLine("{0},{1:d2}/{2:d2}/{3:d4},{4}-{5},{6},{7}",
                                 don.ma,
                                 don.ngayMuon.ngay,
                                 don.ngayMuon.thang,
                                 don.ngayMuon.nam,
                                 don.tietBDMuon,
                                 don.tietKTMuon,
                                 don.tenNgMuon,
                                 don.phongMuon);
                Console.Write(".");
            }

            Console.WriteLine("Xong");
            writer.Close();
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

        // Ham kiem tra xem ngay/thang/nam va cac tiet muon co bi trung voi cac don dang ky da co hay khong
        // Bi trung khi 
        static bool LaTrungNgayTietPhongMuon(NgayThang ngaymuon, int tietBD, int tietKT, string phongMuon)
        {
            foreach (DonMuonPhong don in listDonMP)
                if (LaTrungNgayThangNam(ngaymuon, don.ngayMuon)
                    && (LaThuocDoan(tietBD, don.tietBDMuon, don.tietKTMuon)
                    || LaThuocDoan(tietKT, don.tietBDMuon, don.tietKTMuon))
                    && (phongMuon == don.phongMuon))
                    return true;
            return false;
        }

        // Ham kiem tra gia tri a thuoc doan [b,c] hay khong
        static bool LaThuocDoan(int a, int b, int c)
        {
            if ((a >= b) && (a <= c))
                return true;
            return false;
        }

        // Ham kiem tra trung ngay thang nam
        static bool LaTrungNgayThangNam(NgayThang ngaymuon, NgayThang ngaythang)
        {
            if (ngaymuon.ngay == ngaythang.ngay 
                && ngaymuon.thang == ngaythang.thang 
                && ngaymuon.nam == ngaythang.nam)
                return true;
            return false;
        }

        // Ham kiem tra danh sach rong
        static bool LaRong(string mucdich)
        {
            if (listDonMP.Count == 0)
            {
                Console.WriteLine("\nCo so du lieu khong co don dang ky nao {0}.", mucdich);
                Console.ReadKey();
                return true;
            }
            return false;
        }

        // xu ly nhap ma don dang ky muon phong
        static int XulyNhapMa(string mucdich)
        {
            int ma;
            Console.Write("Nhap ma don dang ky {0} : ", mucdich);
            while (!int.TryParse(Console.ReadLine(), out ma)
                  || ma < 0)
            {
                Console.WriteLine("Ma don dang ky ban nhap khong dung.");
                if (XuLyCauHoiYesNo("Ban co muon nhap lai khong ? (Y/N) : "))
                    Console.Write("Moi ban nhap lai :");
                else
                    break;
            }
            return ma;
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

        // Ham tra ra vi tri cua mot don dang ky theo ma dang ky
        static int LayIndexTheoMa(int ma)
        {
            for (int i = 0; i < listDonMP.Count; i++)
                if (ma == listDonMP[i].ma)
                    return i;
            return -1;
        }

        // Ham tra ra true neu ngay thang nam cua ngay1 la nho hon hoac bang ngay thang nam cua ngay 2
        // dua tren ngay tuyet doi
        static bool LaNgayTruoc(NgayThang ngay1, NgayThang ngay2)
        {
            int ngaytuyetdoi1 = NgayTuyetDoi(ngay1.ngay, ngay1.thang, ngay1.nam);
            int ngaytuyetdoi2 = NgayTuyetDoi(ngay2.ngay, ngay2.thang, ngay2.nam);
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

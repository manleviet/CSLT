using System;
using System.Collections.Generic;
using System.IO;

namespace QLMPM
{
    #region Vung khai bao kieu du lieu
    // cau truc luu tru cac flashcard
    struct Flashcard
    {
        public string tu;
        public string nghia;
        public bool dahoc;
        public bool dakiemtra;
        public bool dathuoc;
    }

    // Kieu enum dung de biet muon thong ke kieu flashcard nao
    enum KieuThongKe { DaHoc, DaKiemTra, DaThuoc };

    // Kieu cu phap the hien cu phap dong lenh khi goi chuong trinh
    // Dang 1 : Flashcard.exe
    // Dang 2 : Flashcard.exe -h
    // Dang 3 : Flashcard.exe <num> // them du lieu voi so luong xac dinh
    // Dang 4 : Flashcard.exe <path> // them du lieu tu file
    enum KieuCuPhap { Dang1, Dang2, Dang3, Dang4 };
    #endregion

    class Program
    {
        #region Khai bao bien
        // bien listFC de luu tru danh sach cac Flashcard
        static List<Flashcard> listFC;
        // bien kieuCP de nhan dang cu phap dong lenh
        static KieuCuPhap kieuCP;
        // duong dan den file chua toan bo flashcard cua ung dung
        static string filepath = "data.txt";
        #endregion

        #region Main
        static void Main(string[] args)
        {
            // khoi tao listFC;
            listFC = new List<Flashcard>();

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

            DocFile(filepath);

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
                    ThemFCMoi(int.Parse(args[0]));
                    // Hien thi Menu
                    XuLyMenu();
                    break;
                case KieuCuPhap.Dang4:
                    // Them du lieu tu file
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
            Console.WriteLine("UNG DUNG FLASHCARD");
            Console.WriteLine("Cu phap cau lenh theo 1 trong 4 dang sau :");
            Console.WriteLine("Flashcard.exe"); // Dang 1
            Console.WriteLine("Flashcard.exe -h"); // Dang 2
            Console.WriteLine("Flashcard.exe <num>"); // Dang 3
            Console.WriteLine("Flashcard.exe <path>"); // Dang 4
            Console.WriteLine();
        }
        #endregion

        #region Xu ly menu
        // In menu cua chuong trinh
        static void InMenu()
        {
            Console.WriteLine();
            Console.Write("UNG DUNG FLASHCARD");
            Console.WriteLine();
            Console.WriteLine("1 - Them flashcard moi");
            Console.WriteLine("2 - Them flashcard moi tu file");
            Console.WriteLine("3 - Tim kiem");
            Console.WriteLine("4 - Xoa flashcard");
            Console.WriteLine("5 - Hoc");
            Console.WriteLine("6 - Kiem tra");
            Console.WriteLine("7 - In danh sach flashcard");
            Console.WriteLine("8 - Thong ke cac flashcard");
            Console.WriteLine("9 - Thoat");
            Console.WriteLine("Hay chon so tu 1 den 9 phu hop voi chuc nang tuong ung.");
            Console.Write("Ban chon so : ");
        }

        // ham xu ly menu
        static void XuLyMenu()
        {
            int menu;
            while (true)
            {
                InMenu(); // in menu ra man hinh
                menu = XulyChonMenu(1, 9); // cho nguoi su dung chon menu

                switch (menu)
                {
                    case 1: // Them flashcard moi
                        ThemFCMoi();
                        break;
                    case 2: // Them flashcard moi tu file
                        Console.Write("\nNhap duong dan den tap tin chua flashcard : ");
                        string filename = Console.ReadLine();
                        DocFile(filename);
                        break;
                    case 3: // Tim kiem
                        TimKiem();
                        break;
                    case 4: // Xoa
                        XoaFC();
                        break;
                    case 5: // Hoc
                        Hoc();
                        break;
                    case 6: // Kiem tra
                        KiemTra();
                        break;
                    case 7: // in danh sach flashcard
                        InTatCa();
                        break;
                    case 8: // Thong ke flashcard
                        ThongKeFC();
                        break;
                    case 9: // thoat khoi chuong trinh
                        if (XuLyCauHoiYesNo("Ban co chac muon thoat chuong trinh ? (Y/N): "))
                        {
                            Console.WriteLine("\nCam on ban da su dung chuong trinh !");
                            Console.ReadKey();

                            GhiFile(filepath);
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
        #endregion

        // XU LY THEM FLASHCARD
        #region Them flashcard
        // Ham cho phep nhap them n flashcard moi
        // ham nhap voi so luong nhap xac dinh truoc
        static void ThemFCMoi(int n)
        {
            Console.WriteLine("\nTHEM DON DANG KY MOI");

            for (int i = 0; i < n; i++)
            {
                Console.WriteLine("{0}.", i + 1);
                Flashcard fc = ThemMotFCMoi(); // nhap cho mot flashcard
                listFC.Add(fc);
            }
        }

        // Ham cho phep nhap vao cac flashcard moi
        // viec nhap chi dung lai khi nguoi su dung muon
        static void ThemFCMoi()
        {
            Console.WriteLine("\nTHEM FLASHCARD MOI");

            do
            {
                Flashcard fc = ThemMotFCMoi(); // nhap thong tin cho mot flashcard
                listFC.Add(fc);
                // hoi nguoi su dung co muon tiep tuc khong
            } while (XuLyCauHoiYesNo("\nBan co muon nhap them flashcard khac khong ? (Y/N) : "));
        }

        // Ham xu ly nhap thong tin cho mot flashcard
        static Flashcard ThemMotFCMoi()
        {
            Flashcard fc = new Flashcard(); // khoi tao

            Console.WriteLine("\nMoi ban nhap thong tin flashcard :");
            Console.Write("Tu : ");
            fc.tu = Console.ReadLine();
            Console.Write("Nghia : ");
            fc.nghia = Console.ReadLine();
            fc.dahoc = false;
            fc.dathuoc = false;
            fc.dakiemtra = false;

            return fc;
        }
        #endregion

        // TIM KIEM FLASHCARD
        #region Tim kiem
        // Ham tim kiem theo tu
        static void TimKiem()
        {
            Console.WriteLine("\nTIM KIEM THEO TU");

            // neu danh sach rong thi bo qua
            if (LaRong("de tim kiem"))
                return;

            int index;
            do
            {
                Console.Write("Nhap tu muon tim nghia : ");
                string tu = Console.ReadLine();

                if ((index = LayIndexTheoTu(tu)) == -1) // neu khong co
                    Console.WriteLine("Tu ban muon tim khong co trong co so du lieu.");
                else // nguoc lai
                    InMotFC(listFC[index]);
            } while (XuLyCauHoiYesNo("\nBan co tiep tuc tim nghia cua tu khac khong? (Y/N):"));
        }
        #endregion

        #region Xoa
        // Ham xu ly xoa flashcard
        static void XoaFC()
        {
            Console.WriteLine("\nXOA FLASHCARD");
            // neu la rong thi bo qua
            if (LaRong("de xoa")) return;

            int index;
            Console.WriteLine("\nBan can xoa flashcard nao ?");
            do
            {
                Console.Write("Nhap tu cua flashcard muon xoa : ");
                string tu = Console.ReadLine();

                if ((index = LayIndexTheoTu(tu)) == -1) // neu tu khong ton tai trong danh sach
                    Console.WriteLine("Tu ban nhap khong co trong co so du lieu.");
                else // nguoc lai
                {
                    Console.WriteLine("Flashcard voi tu {0} co thong tin nhu sau :", listFC[index].tu);
                    // in thong tin hien co cua flashcard muon xoa
                    InMotFC(listFC[index]);

                    // xac dinh lai y muon xoa flashcard cua nguoi dung
                    if (XuLyCauHoiYesNo("\nBan co that su muon xoa flashcard nay ? (Y/N):"))
                    {
                        listFC.RemoveAt(index); // xoa flashcard khoi danh sach
                        Console.WriteLine("Flashcard voi tu {0} da duoc xoa.", tu);
                        Console.ReadKey();
                    }
                }
            } while (XuLyCauHoiYesNo("\nBan co tiep tuc xoa nhung flashcard khac? (Y/N):"));
        }
        #endregion

        #region Chon Flashcard
        // Chon ra cac flashcard chua hoc
        static void LayFCChuaHoc(int soChuaHoc, ref List<Flashcard> listChon, out int sothieu)
        {
            int demChuaHoc = 0;
            foreach (Flashcard fc in listFC)
            {
                if (demChuaHoc == soChuaHoc) break;
                if (!fc.dahoc)
                {
                    listChon.Add(fc);
                    demChuaHoc++;
                }
            }
            sothieu = soChuaHoc - demChuaHoc;
        }

        // Chon ra cac flashcard da hoc nhung chua kiem tra
        static void LayFCChuaKT(int soChuaKT, ref List<Flashcard> listChon, out int sothieu)
        {
            int demChuaKT = 0;
            foreach (Flashcard fc in listFC)
            {
                if (demChuaKT == soChuaKT) break;
                if (fc.dahoc && !fc.dakiemtra)
                {
                    listChon.Add(fc);
                    demChuaKT++;
                }
            }
            sothieu = soChuaKT - demChuaKT;
        }

        // Chon ra cac flashcard da kiem tra nhung chua thuoc
        static void LayFCChuaThuoc(int soChuaThuoc, ref List<Flashcard> listChon, out int sothieu)
        {
            int demChuaThuoc = 0;
            foreach (Flashcard fc in listFC)
            {
                if (demChuaThuoc == soChuaThuoc) break;
                if (fc.dakiemtra && !fc.dathuoc)
                {
                    listChon.Add(fc);
                    demChuaThuoc++;
                }
            }
            sothieu = soChuaThuoc - demChuaThuoc;
        }

        // Chon ra cac flashcard da thuoc mot cach ngau nhien
        static void LayFCDaThuoc(int soDaThuoc, ref List<Flashcard> listChon)
        {
            // loc ra tat ca cac flashcard da thuoc
            List<Flashcard> listDaThuoc = new List<Flashcard>();
            foreach (Flashcard fc in listFC)
                if (fc.dathuoc)
                    listDaThuoc.Add(fc);

            Random r = new Random();
            // neu so flashcard da loc ra nho hon hoac bang soDaThuoc
            if (listDaThuoc.Count <= soDaThuoc)
                listChon.AddRange(listDaThuoc); // day het cac flashcard da loc vao listChon
            else // chi chon cho du so luong
            {
                for (int i = 0; i < soDaThuoc; i++)
                {
                    int index = r.Next(1, listDaThuoc.Count);
                    listChon.Add(listDaThuoc[index]);
                    listDaThuoc.RemoveAt(index);
                }
            }
        }

        // Chon ra cac flashcard de hoc
        static void LayFCDeHoc(int sotu, ref List<Flashcard> listHoc)
        {
            int soChuaHoc = sotu, soChuaThuoc = 0, thieu, soConThieu;
            if (DaCoHoc() && DaCoKiemTra()) // neu da hoc va da kiem tra roi
            { // thi chia so luong tu hoc theo cong thuc cho moi loai
                soChuaHoc = sotu * 2 / 3;
                soChuaThuoc = sotu - soChuaHoc;

                // truong hop so flashcard chua thuoc kha nho
                int soFCChuaThuoc = SoFCChuaThuoc();
                if (soFCChuaThuoc < soChuaThuoc)
                {
                    soChuaHoc = soChuaThuoc - soFCChuaThuoc;
                    soChuaThuoc = soFCChuaThuoc;
                }
            }

            // Chon cac flashcard chua hoc
            LayFCChuaHoc(soChuaHoc, ref listHoc, out thieu);
            soConThieu = thieu;
            // Chon cac flashcard da kiem tra ma chua thuoc
            LayFCChuaThuoc(soChuaThuoc, ref listHoc, out thieu);
            soConThieu += thieu;
            // Chon cac flashcard da hoc nhung chua kiem tra
            LayFCChuaKT(thieu, ref listHoc, out thieu);
        }

        // Chon ra cac flashcard de kiem tra
        static void LayFCDeKiemTra(ref List<Flashcard> listKT)
        {
            int thieu;
            LayFCChuaKT(5, ref listKT, out thieu);
            LayFCChuaThuoc(5, ref listKT, out thieu);
        }
        #endregion

        #region Hoc
        static void Hoc()
        {
            Console.WriteLine("\nHOC");
            // neu la rong thi bo qua
            if (LaRong("de hoc")) return;

            // nhap so luong tu can hoc, tu 10 - 20 tu
            int sotu = XulyNhapSo();

            List<Flashcard> listHoc = new List<Flashcard>();
            LayFCDeHoc(sotu, ref listHoc);

            // Hoc
            for (int i = 0; i < listHoc.Count; i++)
            {
                Flashcard fc = listHoc[i];

                Console.WriteLine("{0}.", i + 1);
                InMotFC(fc);
                fc.dahoc = true;
                ThayThe(fc);
                Console.ReadKey(); // bam phim bat ky de hoc tiep
            }
        }

        // Thay the flashcard voi tu da co bang flashcard moi
        static void ThayThe(Flashcard fcmoi)
        {
            int index;
            if ((index = LayIndexTheoTu(fcmoi.tu)) != -1)
            {
                listFC.RemoveAt(index);
                listFC.Insert(index, fcmoi);
            }
        }

        // Ham xu ly nhap so tu 10 - 20
        // la so tu muon hoc
        static int XulyNhapSo()
        {
            int num;
            Console.Write("Nhap nhap so tu muon hoc : ");
            while (!int.TryParse(Console.ReadLine(), out num)
                  || num < 10
                  || num > 20)
            {
                Console.WriteLine("Moi lan chi hoc tu 10 - 20 tu. Moi ban nhap lai : ");
            }
            return num;
        }
        #endregion

        #region Kiem Tra
        static void KiemTra()
        {
            Console.WriteLine("\nKIEM TRA");
            // neu la rong thi bo qua
            if (LaRong("de kiem tra")) return;

            if (!DaCoHoc())
            {
                Console.Write("Ban chua hoc flashcard nao ca!");
                Console.ReadKey();
                return;
            }

            List<Flashcard> listKT = new List<Flashcard>();
            LayFCDeKiemTra(ref listKT);

            if (listKT.Count == 0)
            {
                Console.WriteLine("Ban da thuoc het tat ca cac flashcard.");
                Console.ReadKey();
                return;
            }

            int demDung = 0;
            for (int i = 0; i < listKT.Count; i++)
            {
                Flashcard fc = listKT[i];

                Console.WriteLine("{0}.", i + 1);
                fc.dakiemtra = true;
                Console.WriteLine("Nghia : {0}", fc.nghia);
                Console.Write("Tu : ");
                string tu = Console.ReadLine();

                if (tu == fc.tu)
                {
                    Console.WriteLine("DUNG ROI!");
                    fc.dathuoc = true;
                    demDung++;
                }
                else
                    Console.WriteLine("SAI ROI, tu dung la: {0}", fc.tu);

                ThayThe(fc);
                Console.ReadKey(); // bam phim bat ky de kiem tra tiep
            }
            Console.WriteLine("Ban thuoc {0} tu / {1} so tu kiem tra.", demDung, listKT.Count);
            Console.ReadKey();
        }
        #endregion

        #region In an
        // Ham xu ly in thong tin cua tat ca cac flashcard
        static void InTatCa()
        {
            Console.WriteLine("\nIN TAT CA CAC FLASHCARD");

            Console.WriteLine("Co so du lieu co tat ca {0} flashcard.", listFC.Count);
            for (int i = 0; i < listFC.Count; i++)
            {
                Console.WriteLine("{0}.", i + 1);
                InMotFC(listFC[i], true);
                if ((i + 1) % 5 == 0) // cu in duoc 5 flashcard la dung man hinh lai
                    Console.ReadKey(); // bam phim bat ky de in 5 don tiep theo
            }
            Console.ReadKey();
        }

        // Ham xu ly in thong tin mot flashcard
        // Flashcard fc : la flashcard can in
        static void InMotFC(Flashcard fc, bool indaydu = false)
        {
            Console.WriteLine("Tu : {0}", fc.tu); // in tu
            Console.WriteLine("Nghia : {0}", fc.nghia); // in nghia cua tu
            if (indaydu)
            {
                Console.WriteLine(fc.dahoc ? "Da hoc" : "Chua hoc");
                Console.WriteLine(fc.dathuoc ? "Da thuoc" : "Chua thuoc");
                Console.WriteLine(fc.dakiemtra ? "Da kiem tra" : "Chua kiem tra");
            }
        }
        #endregion

        #region Thong ke
        static void ThongKeFC()
        {
            Console.WriteLine("\nTHONG KE FLASHCARD");

            Console.WriteLine("Co so du lieu co tat ca {0} flashcard.", listFC.Count);
            Console.WriteLine("So flashcard da hoc: {0}", ThongKeFCTheoKieu(KieuThongKe.DaHoc));
            Console.WriteLine("So flashcard da kiemtra: {0}", ThongKeFCTheoKieu(KieuThongKe.DaKiemTra));
            Console.WriteLine("So flashcard da thuoc: {0}", ThongKeFCTheoKieu(KieuThongKe.DaThuoc));
            Console.ReadKey();
        }

        static bool DaCoHoc()
        {
            if (ThongKeFCTheoKieu(KieuThongKe.DaHoc) > 0)
                return true;
            else
                return false;
        }

        static bool DaCoKiemTra()
        {
            if (ThongKeFCTheoKieu(KieuThongKe.DaKiemTra) > 0)
                return true;
            else
                return false;
        }

        static int ThongKeFCTheoKieu(KieuThongKe kieu)
        {
            int dem = 0;
            foreach (Flashcard fc in listFC)
                switch (kieu)
                {
                    case KieuThongKe.DaHoc:
                        if (fc.dahoc) dem++;
                        break;
                    case KieuThongKe.DaKiemTra:
                        if (fc.dakiemtra) dem++;
                        break;
                    case KieuThongKe.DaThuoc:
                        if (fc.dathuoc) dem++;
                        break;
                }
            return dem;
        }

        static int SoFCChuaThuoc()
        {
            int dem = 0;
            foreach (Flashcard fc in listFC)
                if (fc.dakiemtra && !fc.dathuoc) dem++;
            return dem;
        }
        #endregion

        #region Xu ly file
        // Ham xu ly doc file
        static void DocFile(string filename = null)
        {
            Console.WriteLine("\nDOC DU LIEU TU FILE {0}", filename);

            int soluongcu = listFC.Count;
            if (listFC.Count != 0)
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
                Flashcard fc;
                // tach cac thanh phan trong dong doc duoc
                // va gan gia tri vao bien cau truc don
                if (!TachGiaTri(dong, out fc))
                { // neu qua trinh tach bi loi thi bao loi
                    Console.WriteLine("File bi loi!", reader);
                    return; // thoat khoi ham Doc file
                }

                listFC.Add(fc); // them don vao danh sach
            }

            Console.WriteLine("Da them duoc {0} flashcard vao CSDL", listFC.Count - soluongcu);

            // Dong tap tin
            reader.Close();
        }

        // Ham co nhiem vu tach chuoi doc duoc tu file
        // gan gia tri vao bien fc
        static bool TachGiaTri(string input, out Flashcard fc)
        {
            fc = new Flashcard();
            string[] items = input.Split('|');

            // phai chua 5 thanh phan
            if (items.Length != 5) return false;

            fc.tu = items[0];
            fc.nghia = items[1];

            if (!bool.TryParse(items[2], out fc.dahoc)) return false;
            if (!bool.TryParse(items[3], out fc.dathuoc)) return false;
            if (!bool.TryParse(items[4], out fc.dakiemtra)) return false;

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
            foreach (Flashcard fc in listFC)
            {
                writer.WriteLine("{0}|{1}|{2}|{3}|{4}",
                                 fc.tu,
                                 fc.nghia,
                                 fc.dahoc,
                                 fc.dathuoc,
                                 fc.dakiemtra);
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

        // Ham kiem tra danh sach rong
        static bool LaRong(string mucdich)
        {
            if (listFC.Count == 0)
            {
                Console.WriteLine("\nCo so du lieu khong co flashcard nao {0}.", mucdich);
                Console.ReadKey();
                return true;
            }
            return false;
        }

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
        static int LayIndexTheoTu(string tu)
        {
            for (int i = 0; i < listFC.Count; i++)
                if (tu == listFC[i].tu)
                    return i;
            return -1;
        }
        #endregion
    }
}
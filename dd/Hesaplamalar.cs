namespace dd
{
    public class Hesaplamalar
    {
        Form1 form1;

        public Hesaplamalar(Form1 form1)
        {
            this.form1 = form1;
        }

        /*****************************************************************************************
        *  @brief:      Dikdörtgen ile alakalı başlangıç koordinatları, genişlik, yükseklik,
        *               çizdirilmesi için gerekli şartı sağlayıp sağlamadığı, hangi path için
        *               hesaplandığı, köşe noktaları bilgilerini ve alanını içerir
        *  @elements:   int[] coordsX: Dikdörtgenin köşelerinin x koordinatı
        *               int[] coordsY: Dikdörtgenin köşelerinin y koordinatı
        *               int w: Genişlik
        *               int h: Yükseklik
        *               bool is_it_OK: Çizdirilmesi için uygun olup olmadığı bilgisi
        *               int path_no: Hani path için hesaplanıyor
        *               int area: Dikdörtgenin alanı
        *  @comment:    Dikdörtgenin başlangıç konumu (coordsX[0],coordsY[0]) noktasıdır ve
        *               köşe noktaları saat yönünde sırayla devam etmektedir
        *               Genişlik ve yükseklik baz alınarak çizim yapılacaksa; genişlik, başlangıç
        *               noktasından sağa doğru olan uzaklık, yükseklik ise başlangıç noktasından
        *               aşağı doğru olan uzaklık anlamına gelmektedir.
        *****************************************************************************************/
        public struct RectangleSpecs
        {
            private int[] coordsX;
            private int[] coordsY;
            private int w;
            private int h;
            private bool is_it_OK;
            private int path_no;
            private int area;
            public int[] COORDS_X
            {
                get { return coordsX; }
                set { coordsX = value; }
            }
            public int[] COORDS_Y
            {
                get { return coordsY; }
                set { coordsY = value; }
            }
            public int W
            {
                get { return w; }
                set { w = value; }
            }
            public int H
            {
                get { return h; }
                set { h = value; }
            }
            public bool IS_IT_OK
            {
                get { return is_it_OK; }
                set { is_it_OK = value; }
            }
            public int PATH_NO
            {
                get { return path_no; }
                set { path_no = value; }
            }
            public int AREA
            {
                get { return area; }
                set { area = value; }
            }
        }

        /*****************************************************************************************
         *  @brief:     Girilen koordinat değerlerinden en büyüğünü bulur.
         *  @input:     int[] coord: Bir eksenin koordinat değerlerinin bulunduğu array
         *  @output:    int max: Girdi olarak verilen değerlerin en büyüğü
         *****************************************************************************************/
        int max_coords(int[] coord)
        {
            int max = 0;
            for (int a = 0; a < coord.Length; a++)
            {
                if (a == 0) max = coord[a];
                if (coord[a] > max)
                {
                    max = coord[a];
                }
            }
            return max;
        }

        /*****************************************************************************************
         *  @brief:     Girilen koordinat değerlerinden en küçüğünü bulur.
         *  @input:     int[] coord: Bir eksenin koordinat değerlerinin bulunduğu array
         *  @output:    int min: Girdi olarak verilen değerlerin en küçüğü
         *****************************************************************************************/
        int min_coords(int[] coord)
        {
            int min = 0;
            for (int a = 0; a < coord.Length; a++)
            {
                if (a == 0) min = coord[a];
                if (coord[a] < min)
                {
                    min = coord[a];
                }
            }
            return min;
        }

        /*****************************************************************************************
        *  @brief:      Dikdörtgenin (X,Y) başlangıç konumunu, yüksekliğini, genişliğini, alanını 
        *               ve köşe noktalarının koordinatlarını hesaplar
        *  @input:      int maxX: Büyük olan x koordinat değeri
        *               int minX: Küçük olan x koordinat değeri
        *               int maxY: Büyük olan y koordinat değeri
        *               int minY: Küçük olan y koordinat değeri
        *               int offset: Dikdörtgen köşe noktaları için gerekli ayar değeri
        *  @output:     RectangleSpecs rectangleScpecs_s: İlgili değerler RectangleSpecs struct'ına
        *               yazdırılır ve çıktı olarak bu struct verilir
        *  @comment:    Eğer ki dikdörtgen oluşturmayacak bir şekil çizilirse (dümdüz bir path veya 
        *               tek bir nokta) bu şekli dikdörtgene benzetmek için gerekli işlemleri de 
        *               içermektedir. Bu işlemi offset değeri ile ayarlamaktadır.
        *               Offset değerini örnekle açıklamak gerekirse; 
        *               offset = 0 olduğu durumda ilgili path'in sınır noktaları, çizilecek olan
        *               dikdörtgenin tam olarak kenarların bulunmaktadır.
        *               offset = 1 olduğu durumda ise dikdörtgen, path'in sınır noktalarından 1 birim
        *               dışarıda olacak şekilde çizilecektir. Bu durumda path sınır noktaları
        *               dikdörtgenin alanı içerisinde olacaktır.
        *               offset = -1 olması durumunda ise path sınır noktaları dikdörtgenin 1 birim
        *               dışında olacaktır.
        *****************************************************************************************/
        RectangleSpecs calculate_rectangle(int maxX, int minX, int maxY, int minY, int offset) 
        {
            RectangleSpecs rectangleSpecs_s = new RectangleSpecs();
            if (offset == 0 && ((maxX == minX) || (maxY == minY)))
            {
                offset = 1;
            }
            maxX = maxX + offset;
            minX = minX - offset;
            maxY = maxY + offset;
            minY = minY - offset;
            rectangleSpecs_s.W = (maxX - minX);
            rectangleSpecs_s.H = (maxY - minY);
            rectangleSpecs_s.AREA = rectangleSpecs_s.W * rectangleSpecs_s.H;
            rectangleSpecs_s.COORDS_X = new int[4];
            rectangleSpecs_s.COORDS_Y = new int[4];
            rectangleSpecs_s.COORDS_X[0] = minX;
            rectangleSpecs_s.COORDS_Y[0] = minY; //koordinat sistemi y ekseni ters, yani aşağı inildikçe artıyor, normal değeri: maxY
            rectangleSpecs_s.COORDS_X[1] = rectangleSpecs_s.COORDS_X[0] + rectangleSpecs_s.W;
            rectangleSpecs_s.COORDS_Y[1] = rectangleSpecs_s.COORDS_Y[0];
            rectangleSpecs_s.COORDS_X[2] = rectangleSpecs_s.COORDS_X[1];
            rectangleSpecs_s.COORDS_Y[2] = rectangleSpecs_s.COORDS_Y[1] + rectangleSpecs_s.H;
            rectangleSpecs_s.COORDS_X[3] = rectangleSpecs_s.COORDS_X[0];
            rectangleSpecs_s.COORDS_Y[3] = rectangleSpecs_s.COORDS_Y[2];
            return rectangleSpecs_s;
        }

        /*****************************************************************************************
        *  @brief:      Dikdörtgen oluşturmak için gerekli tüm hesapları yapar (ana fonksiyon)
        *  @input:      int area: Kullanıcıdan alınan alan bilgisi
        *               int path_no: İşlemlerin yapılacağı path numarası
        *               int[] x: İlgili path'in x koordinatları
        *               int[] y: İlgili path'in y koordinatları
        *  @output:     RectangleSpecs rectangleScpecs_s: RectangleSpecs struct'ı doldurulur ve
        *               kullanılmak üzere hazır olarak çıktı verilir
        *****************************************************************************************/
        public RectangleSpecs calculate(int area, int path_no, int[] x, int[] y, int offset)
        {
            RectangleSpecs rectangleSpecs_s = new RectangleSpecs();
            int maxX = max_coords(x);
            int maxY = max_coords(y);
            int minX = min_coords(x);
            int minY = min_coords(y);
            rectangleSpecs_s = calculate_rectangle(maxX, minX, maxY, minY, offset);
            if (area >= rectangleSpecs_s.AREA)
            {
                rectangleSpecs_s.IS_IT_OK = true;
            }
            else
            {
                rectangleSpecs_s.IS_IT_OK = false;
            }
            rectangleSpecs_s.PATH_NO = path_no;
            return rectangleSpecs_s;
        }
    }
}

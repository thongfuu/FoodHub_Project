# FoodHub - แนวอธิบายโค้ดสำหรับพรีเซนต์

เอกสารนี้ใช้เป็นสคริปต์เตรียมพรีเซนต์และตอบคำถามจากอาจารย์ โดยอธิบายโครงสร้างโปรเจกต์ โค้ดแต่ละส่วน เหตุผลของการออกแบบ และจุดที่ควรระวังเวลา demo

## 1. ภาพรวมระบบ

โปรเจกต์ FoodHub เป็นระบบจองโต๊ะร้านอาหาร แบ่งเป็น 3 ส่วนหลัก

1. `FoodHub/FoodHubApi` คือฝั่ง Server API
2. `FoodHubCustomerUI/FoodHubCustomerUI` คือโปรแกรมฝั่งลูกค้า
3. `FoodHubRestaurantUI/FoodHubRestaurantUI` คือโปรแกรมฝั่งร้านอาหาร

แนวคิดการออกแบบคือแยกหน้าที่ให้ชัดเจน ลูกค้าและร้านอาหารไม่เข้าถึงฐานข้อมูลโดยตรง แต่เรียกผ่าน API กลาง ทำให้ business rules เช่น เช็กโต๊ะเต็ม เช็กเวลาเปิดร้าน อัปเดตสถานะการจอง อยู่ที่เดียวในฝั่ง API

เวลาพรีเซนต์ให้พูดเป็น flow ได้ว่า

ลูกค้า Login -> ดูร้านอาหาร -> ดูรายละเอียดร้าน/โปรโมชัน/รีวิว -> จองโต๊ะ -> ร้านอาหาร Login -> เห็นคำขอจอง -> ยืนยันหรือปฏิเสธ -> ลูกค้าเห็นสถานะ -> เมื่อเสร็จสิ้นสามารถรีวิวได้

## 2. Architecture Design

ระบบใช้โครงสร้างแบบ Client-Server

- Client คือ Windows Forms 2 ตัว ได้แก่ฝั่งลูกค้าและฝั่งร้านอาหาร
- Server คือ ASP.NET Core Web API
- Database คือ PostgreSQL
- ORM คือ Entity Framework Core

เหตุผลที่ออกแบบแบบนี้

- แยกหน้าจอออกจาก logic สำคัญ ทำให้แก้ UI ได้โดยไม่กระทบฐานข้อมูลโดยตรง
- API เป็นศูนย์กลางของกติกาธุรกิจ เช่น การจองต้องไม่เกินความจุร้าน
- ถ้าต่อไปอยากทำเป็นเว็บหรือมือถือ สามารถใช้ API เดิมได้
- Entity Framework ช่วย map class C# เป็นตารางฐานข้อมูล ทำให้โค้ดอ่านง่ายกว่าการเขียน SQL ทุกจุด

## 3. Database Design

ฐานข้อมูลมี 5 ตารางหลัก

### Customers

เก็บข้อมูลลูกค้า เช่น `CustomerId`, `Name`, `Email`, `Phone`, `LateCancelCount`

เหตุผลที่มี `LateCancelCount` เพราะระบบต้องจำจำนวนครั้งที่ลูกค้ายกเลิกช้า เพื่อใช้เป็นประวัติหรือเงื่อนไขในอนาคต

### Restaurants

เก็บข้อมูลร้านอาหาร เช่น `RestaurantId`, `Name`, `OwnerName`, `Category`, `OpenHours`, `AvgRating`, `MaxCapacity`

เหตุผลที่มี `OpenHours` และ `MaxCapacity` เพราะตอนจองต้องตรวจว่าร้านเปิดอยู่ไหม และจำนวนคนเกินความจุหรือไม่

### Bookings

เก็บรายการจอง เช่น ลูกค้าคนไหนจองร้านไหน วันเวลาไหน มากี่คน และสถานะอะไร

สถานะสำคัญคือ

- `Pending` รอร้านยืนยัน
- `Confirmed` ร้านยืนยันแล้ว
- `Rejected` ร้านปฏิเสธ
- `Cancelled` ลูกค้ายกเลิกปกติ
- `LateCancelled` ลูกค้ายกเลิกช้า
- `Completed` ใช้บริการเสร็จแล้ว
- `NoShow` ลูกค้าไม่มาตามนัด

### Reviews

เก็บรีวิวของลูกค้า มีคะแนน 1-5 และข้อความรีวิว โดยผูกกับ `CustomerId`, `RestaurantId` และอาจผูกกับ `BookingId`

เหตุผลที่ `BookingId` เป็น optional เพราะรีวิวสามารถผูกกับรายการจองได้ แต่ระบบยังยืดหยุ่นถ้าไม่มี booking id

### Promotions

เก็บโปรโมชันของร้าน เช่น ชื่อโปรโมชัน รายละเอียด ส่วนลด วันหมดอายุ และเปิดใช้งานอยู่หรือไม่

ความสัมพันธ์โดยรวม

- ลูกค้า 1 คนมี booking ได้หลายรายการ
- ร้าน 1 ร้านมี booking ได้หลายรายการ
- ร้าน 1 ร้านมี promotion ได้หลายรายการ
- ร้าน 1 ร้านมี review ได้หลายรายการ
- ลูกค้า 1 คนเขียน review ได้หลายรายการ

## 4. ฝั่ง API: Program.cs

ไฟล์ `FoodHub/FoodHubApi/Program.cs` เป็นจุดเริ่มต้นของ Server

```csharp
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
```

บรรทัดนี้ตั้งค่าให้ Npgsql จัดการ `DateTime` แบบเข้ากับพฤติกรรมเดิม ช่วยลดปัญหา timezone ตอนส่งวันที่ระหว่าง C# กับ PostgreSQL

```csharp
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
```

ส่วนนี้เปิดใช้ Controller และ Swagger

เหตุผลที่ใช้ Swagger เพราะตอน demo หรือ debug สามารถเปิดหน้าเว็บเพื่อทดสอบ API ได้ทันที เช่น GET ร้านอาหาร หรือ POST booking

```csharp
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<FoodHubDbContext>(options =>
    options.UseNpgsql(connectionString));
```

ส่วนนี้เชื่อม ASP.NET Core กับ PostgreSQL ผ่าน Entity Framework Core

ทำไมใช้ `DbContext` เพราะมันเป็นตัวกลางระหว่างโค้ด C# กับฐานข้อมูล เวลาเขียน `_context.Bookings.ToList()` ระบบจะแปลงเป็น query ไปที่ database

```csharp
app.MapControllers();
app.Run();
```

บอกให้ระบบเปิด endpoint จาก Controller ทั้งหมด แล้วเริ่มรัน server

## 5. appsettings.json

ไฟล์ `FoodHub/FoodHubApi/appsettings.json` เก็บ connection string

```json
"DefaultConnection": "Host=localhost;Database=FoodHubDb;Username=foodhubuser;Password=FoodHubPass123;Port=5432;"
```

เหตุผลที่เก็บไว้ใน config ไม่เขียน hardcode ใน Controller เพราะถ้าเปลี่ยน database หรือ username สามารถแก้ที่ไฟล์ config จุดเดียว

## 6. FoodHubDbContext.cs

ไฟล์ `FoodHub/FoodHubApi/Models/DbContext.cs`

```csharp
public DbSet<Customer> Customers { get; set; }
public DbSet<Restaurant> Restaurants { get; set; }
public DbSet<Booking> Bookings { get; set; }
public DbSet<Review> Reviews { get; set; }
public DbSet<Promotion> Promotions { get; set; }
```

แต่ละ `DbSet` แทนตารางหนึ่งตารางในฐานข้อมูล เช่น `Bookings` แทนตารางรายการจอง

เหตุผลที่ต้องมี class นี้ เพราะ Entity Framework ต้องรู้ว่าในระบบมีตารางอะไรบ้าง และ class ไหน map กับ table ไหน

## 7. Models ฝั่ง API

### Customer.cs

```csharp
[Key]
public int CustomerId { get; set; }
```

กำหนด primary key ของลูกค้า

```csharp
[Required, MaxLength(100)]
public string Name { get; set; } = string.Empty;
```

ชื่อจำเป็นต้องมี และจำกัดความยาวเพื่อไม่ให้ข้อมูลยาวเกิน database

```csharp
public int LateCancelCount { get; set; } = 0;
```

เก็บจำนวนครั้งที่ยกเลิกช้า ค่าเริ่มต้นเป็น 0 เพราะลูกค้าใหม่ยังไม่มีประวัติเสีย

### Restaurant.cs

`Restaurant` เก็บข้อมูลร้าน เช่น ชื่อร้าน เจ้าของ อีเมล เบอร์โทร ที่อยู่ ประเภท เวลาเปิดปิด คะแนนเฉลี่ย และความจุสูงสุด

```csharp
public double? AvgRating { get; set; } = 0;
public int MaxCapacity { get; set; } = 20;
```

`AvgRating` ใช้โชว์คะแนนเฉลี่ยในหน้าลูกค้า ส่วน `MaxCapacity` ใช้คำนวณว่ารับ booking เพิ่มได้ไหม

### Booking.cs

```csharp
public int CustomerId { get; set; }
public Customer? Customer { get; set; }
```

`CustomerId` คือ foreign key ส่วน `Customer` คือ navigation property ใช้ตอน Include เพื่อดึงชื่อลูกค้ามาแสดงได้

```csharp
public int RestaurantId { get; set; }
public Restaurant? Restaurant { get; set; }
```

แนวคิดเดียวกัน แต่เชื่อมกับร้านอาหาร

```csharp
public string Status { get; set; } = "Pending";
```

การจองใหม่เริ่มต้นเป็น `Pending` เพราะร้านต้องยืนยันก่อน

### Review.cs

```csharp
[Range(1, 5)]
public int Rating { get; set; }
```

กำหนดให้คะแนนอยู่ระหว่าง 1-5 เพื่อไม่ให้ข้อมูลผิด เช่น 0 หรือ 10

```csharp
public int? BookingId { get; set; }
```

ใช้ `int?` เพราะรีวิวอาจมีหรือไม่มี booking ที่เกี่ยวข้องก็ได้

### Promotion.cs

```csharp
public bool IsActive { get; set; } = true;
```

โปรโมชันใหม่เปิดใช้งานทันทีเป็นค่า default ทำให้ร้านสร้างแล้วลูกค้าเห็นได้เลย

## 8. Controllers ฝั่ง API

### CustomersController

มี endpoint หลักคือ

```csharp
[HttpGet]
public List<Customer> GetCustomers()
```

ใช้ดึงรายชื่อลูกค้าทั้งหมดไปใส่ dropdown หน้า login ลูกค้า

เหตุผลที่ระบบใช้ dropdown login เพราะเป็นงาน demo/prototype ไม่ต้องทำระบบ password ให้ซับซ้อน แต่ยังจำลองการเลือก user ได้

### RestaurantsController

```csharp
[HttpGet]
public List<Restaurant> GetAllRestaurants()
```

ดึงร้านทั้งหมดให้ลูกค้าเลือกดู

```csharp
[HttpGet("{id}")]
public Restaurant GetRestaurantById(int id)
```

ดึงรายละเอียดร้านเดียว เช่น เวลาเปิดปิดและที่อยู่ ใช้ในหน้ารายละเอียดร้าน

### PromotionsController

```csharp
[HttpPost]
public Promotion CreatePromotion([FromBody] Promotion promotion)
```

ร้านอาหารใช้สร้างโปรโมชันใหม่

```csharp
[HttpGet("restaurant/{restaurantId}")]
public List<Promotion> GetPromotionsByRestaurant(int restaurantId)
```

ลูกค้าและร้านใช้ดูโปรโมชันของร้านนั้น โดยกรอง `IsActive == true`

เหตุผลที่กรอง active เพราะไม่อยากแสดงโปรโมชันที่ปิดใช้งานแล้ว

### ReviewsController

```csharp
public Review CreateReview([FromBody] Review review)
```

รับรีวิวจากลูกค้า แล้วบันทึกลง database

หลังจากเซฟรีวิว มี logic อัปเดตคะแนนเฉลี่ยร้าน

```csharp
var allReviews = _context.Reviews.Where(r => r.RestaurantId == review.RestaurantId).ToList();
double avg = allReviews.Average(r => r.Rating);
restaurant.AvgRating = Math.Round(avg, 1);
```

เหตุผลที่อัปเดต `AvgRating` ทันที เพราะหน้ารายการร้านสามารถโชว์คะแนนเฉลี่ยได้เร็ว ไม่ต้องคำนวณใหม่ทุกครั้งที่เปิดหน้า home

```csharp
[HttpGet("restaurant/{restaurantId}")]
```

ดึงรีวิวของร้าน พร้อม `Include(r => r.Customer)` เพื่อเอาชื่อลูกค้ามาโชว์

### BookingsController

Controller นี้สำคัญที่สุด เพราะมี business rules หลักของระบบ

#### GetCustomerBookings

```csharp
_context.Bookings
    .Include(b => b.Restaurant)
    .Where(b => b.CustomerId == customerId)
    .OrderByDescending(b => b.BookingDate)
```

ใช้ดึงประวัติการจองของลูกค้า พร้อมข้อมูลร้าน และเรียงรายการล่าสุดก่อน

เหตุผลที่ Include Restaurant เพราะหน้าลูกค้าต้องแสดงชื่อร้าน ไม่ใช่แค่ `RestaurantId`

#### GetRestaurantBookings

```csharp
_context.Bookings
    .Include(b => b.Customer)
    .Where(b => b.RestaurantId == restaurantId)
    .OrderBy(b => b.BookingDate)
```

ร้านใช้ดูคิวของตัวเอง พร้อมชื่อลูกค้า และเรียงตามเวลาใกล้ไปไกล

#### UpdateBookingStatus

```csharp
booking.Status = newStatus;
_context.SaveChanges();
```

ใช้ร่วมกันหลายปุ่มของร้าน เช่น Confirm, Reject, Completed, NoShow

เหตุผลที่ทำเป็น endpoint กลาง เพราะทุกปุ่มคือการเปลี่ยนสถานะเหมือนกัน ต่างกันแค่ค่าที่ส่งมา

#### CancelBooking

```csharp
TimeSpan timeUntilBooking = booking.BookingDate - DateTime.UtcNow;
```

คำนวณเวลาที่เหลือก่อนถึงเวลาจอง

```csharp
if (timeUntilBooking <= TimeSpan.FromHours(2))
{
    booking.Status = "LateCancelled";
    if (customer != null) customer.LateCancelCount += 1;
}
else
{
    booking.Status = "Cancelled";
}
```

ถ้ายกเลิกก่อนเวลาจองน้อยกว่าหรือเท่ากับ 2 ชั่วโมง จะถือว่า late cancel และเพิ่มประวัติลูกค้า

เหตุผลของ design นี้คือทำให้ระบบมีความยุติธรรมกับร้านอาหาร เพราะร้านเสียโอกาสรับลูกค้าคนอื่นถ้ายกเลิกใกล้เวลามาก

#### PostBooking

เป็นจุดที่ควรอธิบายละเอียดตอนพรีเซนต์

ขั้นที่ 1 ดึงข้อมูลร้าน

```csharp
var restaurant = _context.Restaurants.Find(booking.RestaurantId);
if (restaurant == null) throw new Exception("ไม่พบร้านอาหาร");
```

ต้องเช็กก่อนว่าร้านมีจริง เพราะ booking ต้องผูกกับร้านที่มีอยู่

ขั้นที่ 2 เช็กเวลาเปิดปิด

```csharp
var hours = restaurant.OpenHours.Split('-');
TimeSpan.TryParse(hours[0].Trim(), out TimeSpan openTime)
TimeSpan.TryParse(hours[1].Trim(), out TimeSpan closeTime)
```

ระบบแยก `OpenHours` เช่น `10:00-22:00` เป็นเวลาเปิดและปิด

```csharp
if (openTime < closeTime)
```

กรณีร้านเปิดปิดในวันเดียว เช่น 10:00-22:00

```csharp
else
```

กรณีร้านเปิดข้ามคืน เช่น 18:00-02:00 ต้องเช็กอีกแบบ เพราะเวลาปิดน้อยกว่าเวลาเปิด

เหตุผลที่เขียนแยก 2 กรณี เพราะร้านอาหารบางร้านเปิดถึงหลังเที่ยงคืน ถ้าใช้ logic ธรรมดาจะมองว่าเวลา 01:00 ผิดทั้งที่จริงยังเปิดอยู่

ขั้นที่ 3 เช็กจำนวนคนในช่วงเวลาใกล้กัน

```csharp
var startTime = booking.BookingDate.AddMinutes(-59);
var endTime = booking.BookingDate.AddMinutes(59);
```

ระบบถือว่าการจองในช่วงประมาณ 1 ชั่วโมงก่อนและหลังเป็น slot เดียวกัน เพื่อกันคนจองแน่นเกินไป

```csharp
var currentBookedCount = _context.Bookings
    .Where(b => b.RestaurantId == booking.RestaurantId &&
                b.BookingDate >= startTime &&
                b.BookingDate <= endTime &&
                b.Status != "Cancelled" && b.Status != "Rejected")
    .Sum(b => b.NumPeople);
```

นับจำนวนคนที่จองไว้แล้ว โดยไม่นับรายการที่ยกเลิกหรือถูกปฏิเสธ

```csharp
if (currentBookedCount + booking.NumPeople > restaurant.MaxCapacity)
```

ถ้ารวม booking ใหม่แล้วเกินความจุร้าน ระบบไม่ให้จอง

ขั้นที่ 4 บันทึก booking

```csharp
booking.Status = "Pending";
booking.CreatedAt = DateTime.UtcNow;
_context.Bookings.Add(booking);
_context.SaveChanges();
```

การจองใหม่เป็น Pending เพื่อให้ร้านมีสิทธิ์ตัดสินใจก่อน

## 9. ฝั่งลูกค้า: Program.cs

ไฟล์ `FoodHubCustomerUI/FoodHubCustomerUI/Program.cs`

```csharp
public static readonly HttpClient ApiClient = new HttpClient { BaseAddress = new Uri("http://localhost:5000/") };
```

ใช้ `HttpClient` ตัวเดียวทั้งโปรแกรมสำหรับเรียก API

เหตุผลคือไม่ต้องสร้าง client ใหม่ทุกหน้า และกำหนด base URL จุดเดียว

```csharp
public static int LoggedInCustomerId = 0;
public static string LoggedInCustomerName = "";
```

เก็บข้อมูลลูกค้าที่ login อยู่

```csharp
public static List<PromotionDTO> ClaimedCoupons = new List<PromotionDTO>();
```

เก็บคูปองที่ลูกค้ากดรับระหว่างใช้งานโปรแกรม

ข้อจำกัดที่ควรรู้: คูปองอยู่ใน memory ถ้าปิดโปรแกรมแล้วข้อมูลจะหาย เพราะยังไม่ได้ออกแบบให้บันทึกคูปองลง database

## 10. CustomerLoginForm

หน้าลูกค้า login

```csharp
ApplyModernLayout();
```

เรียกจัด layout และ theme หลังสร้าง control จาก Designer

```csharp
var response = await Program.ApiClient.GetAsync("api/Customers");
```

ตอนโหลดหน้า จะดึงรายชื่อลูกค้าจาก API มาใส่ dropdown

```csharp
cmbCustomers.DisplayMember = "Name";
cmbCustomers.ValueMember = "CustomerId";
```

แสดงชื่อให้ user เห็น แต่เก็บ id ไว้ใช้ภายในระบบ

```csharp
Program.LoggedInCustomerId = selectedCustomer.CustomerId;
Program.LoggedInCustomerName = selectedCustomer.Name;
```

เมื่อลูกค้าเลือกชื่อแล้วกด login ระบบจำว่าใครกำลังใช้งาน

เหตุผลที่ใช้หน้า login แบบ dropdown เพราะเหมาะกับ demo ในห้องเรียน ไม่ต้องเสียเวลาเรื่อง register/password แต่ยังแสดง flow ผู้ใช้ได้ครบ

## 11. CustomerHomeForm

หน้านี้เป็นหน้าแรกหลังลูกค้า login

```csharp
private List<RestaurantDTO> allRestaurants = new List<RestaurantDTO>();
```

เก็บรายชื่อร้านทั้งหมดไว้ใน memory เพื่อให้ค้นหาได้โดยไม่ต้องเรียก API ใหม่ทุกครั้ง

```csharp
await LoadRestaurants();
```

ตอนเปิดหน้า ดึงร้านทั้งหมดจาก API

```csharp
UpdateGrid(allRestaurants);
```

นำข้อมูลร้านใส่ตาราง

```csharp
var filtered = allRestaurants.Where(r => r.Name.ToLower().Contains(keyword) || r.Category.ToLower().Contains(keyword)).ToList();
```

ค้นหาได้ทั้งชื่อร้านและประเภทอาหาร

```csharp
var detailForm = new RestaurantDetailForm(restId, restName);
detailForm.ShowDialog();
```

ดับเบิลคลิกร้านแล้วเปิดหน้ารายละเอียดแบบ dialog เพื่อให้ผู้ใช้จบงานกับร้านนั้นก่อนกลับมาหน้า home

```csharp
Program.ClaimedCoupons.Clear();
```

ตอน logout ล้างข้อมูลลูกค้าและคูปอง เพื่อไม่ให้ข้อมูลลูกค้าคนก่อนติดไปกับคนถัดไป

## 12. RestaurantDetailForm

หน้ารายละเอียดร้านเป็นหน้าที่รวมหลาย feature

- แสดงข้อมูลร้าน
- แสดงรีวิว
- แสดงโปรโมชัน
- กดรับคูปอง
- จองโต๊ะ

ตัวแปรสำคัญ

```csharp
private int _restaurantId;
private string _restaurantName;
```

เก็บร้านที่ผู้ใช้เลือกจากหน้า home

### OnLoad

```csharp
dtpBookingDate.MinDate = DateTime.Now;
numPeople.Value = 2;
```

ห้ามจองย้อนหลัง และตั้งจำนวนเริ่มต้นเป็น 2 คน เพื่อให้ใช้งานง่าย

```csharp
await LoadRestaurantDetail();
await LoadPromotions();
await LoadReviews();
UpdateCouponDropdown();
```

โหลดข้อมูลที่จำเป็นครบก่อนให้ user ใช้งาน

### LoadRestaurantDetail

เรียก `api/Restaurants/{id}` เพื่อเอาเวลาเปิดปิดและที่อยู่มาแสดง

### LoadPromotions

เรียก `api/Promotions/restaurant/{id}` เพื่อแสดงโปรโมชันของร้านนั้น

มีการซ่อน `PromotionId` และ `RestaurantId` เพราะเป็นข้อมูลระบบ ไม่จำเป็นต้องให้ลูกค้าเห็น

### LoadReviews

เรียก `api/Reviews/restaurant/{id}` แล้วแปลงข้อมูลเป็น object สำหรับแสดงในตาราง

```csharp
ลูกค้า = r.Customer?.Name ?? "ไม่ประสงค์ออกนาม"
```

ถ้าไม่มีข้อมูลลูกค้า จะไม่ให้หน้าจอพัง แต่แสดงเป็นข้อความแทน

```csharp
dgvReviews.Columns["ความคิดเห็น"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
dgvReviews.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
```

ทำให้ข้อความรีวิวยาว ๆ ขึ้นบรรทัดใหม่และตารางอ่านง่าย

### btnClaimCoupon_Click

```csharp
bool alreadyClaimed = Program.ClaimedCoupons.Any(c => c.PromotionId == promo.PromotionId);
```

ตรวจว่ากดรับคูปองซ้ำหรือยัง

```csharp
Program.ClaimedCoupons.Add(promo);
UpdateCouponDropdown();
```

ถ้ายังไม่เคยรับ จะเพิ่มเข้า “กระเป๋าคูปอง” แล้วรีเฟรช dropdown ทันที

### btnBook_Click

```csharp
var newBooking = new
{
    customerId = Program.LoggedInCustomerId,
    restaurantId = _restaurantId,
    bookingDate = dtpBookingDate.Value.ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture),
    numPeople = (int)numPeople.Value
};
```

สร้าง object booking ส่งไป API

เหตุผลที่ใช้ anonymous object เพราะข้อมูลที่ส่งไปมีแค่ field ที่ API ต้องการ ไม่จำเป็นต้องสร้าง class ใหม่ในจุดนี้

```csharp
var response = await Program.ApiClient.PostAsync("api/Bookings", content);
```

ส่งคำขอจองให้ server ตรวจ rule ทั้งหมด

ถ้า success จะแจ้งว่ารอร้านยืนยัน ถ้า fail จะแจ้งว่าอาจโต๊ะเต็มหรืออยู่นอกเวลาเปิดปิด

## 13. MyBookingsForm

หน้านี้ใช้ดูประวัติการจอง ยกเลิก และเขียนรีวิว

```csharp
api/Bookings/customer/{Program.LoggedInCustomerId}
```

ดึงเฉพาะรายการของลูกค้าที่ login อยู่

```csharp
RestaurantId = b.RestaurantId
```

เก็บ `RestaurantId` ไว้ในตารางแต่ซ่อนไว้ เพื่อใช้ตอนกดรีวิว

```csharp
if (status != "Pending" && status != "Confirmed")
```

ยกเลิกได้เฉพาะรายการที่ยังรอหรือยืนยันแล้วเท่านั้น ถ้า Completed, Cancelled, Rejected แล้วจะไม่ให้ยกเลิกซ้ำ

```csharp
if (status != "Completed")
```

รีวิวได้เฉพาะรายการที่ใช้บริการเสร็จแล้ว เพื่อป้องกันรีวิวก่อนเข้าร้านจริง

```csharp
dgvBookings_CellFormatting
```

เปลี่ยนสีสถานะ เช่น ยกเลิกเป็นสีแดง ยืนยันแล้วเป็นสีเขียว เพื่อให้ผู้ใช้ดูสถานะเร็วขึ้น

## 14. ReviewDialogForm

หน้าต่างเขียนรีวิว

```csharp
private int _restId;
```

จำว่ากำลังรีวิวร้านไหน

```csharp
var reviewData = new
{
    customerId = Program.LoggedInCustomerId,
    restaurantId = _restId,
    rating = (int)numRating.Value,
    comment = txtComment.Text
};
```

เตรียมข้อมูลรีวิวส่ง API

```csharp
this.DialogResult = DialogResult.OK;
```

ส่งสัญญาณกลับไปหน้า `MyBookingsForm` ว่ารีวิวสำเร็จแล้ว

## 15. CustomerUiTheme

ไฟล์นี้เป็น design system ของฝั่งลูกค้า

```csharp
public static readonly Color Primary = Color.FromArgb(45, 101, 82);
public static readonly Color Accent = Color.FromArgb(189, 139, 72);
public static readonly Color Danger = Color.FromArgb(178, 65, 65);
```

กำหนดสีหลัก สีเน้น และสีอันตราย เช่นปุ่มยกเลิก

เหตุผลของ design

- สีเขียวให้ความรู้สึกเกี่ยวกับอาหาร/ความเป็นมิตร
- สีพื้นอ่อนช่วยให้อ่านตารางนาน ๆ ไม่ล้า
- สีแดงใช้เฉพาะ action ที่มีความเสี่ยง เช่น ยกเลิก
- กำหนด font และ style กลาง เพื่อให้ทุกหน้าดูเป็นระบบเดียวกัน

```csharp
StyleGrid(DataGridView grid)
```

ใช้จัดตารางทุกหน้าให้เหมือนกัน เช่น header สีเขียว แถวสลับสี อ่านง่าย ห้ามแก้ข้อมูลโดยตรง

เหตุผลที่ห้ามแก้ในตาราง เพราะข้อมูลควรถูกเปลี่ยนผ่านปุ่มและ API ไม่ใช่ให้ user พิมพ์แก้ cell ตรง ๆ

## 16. ฝั่งร้านอาหาร: Program.cs

ไฟล์ `FoodHubRestaurantUI/FoodHubRestaurantUI/Program.cs`

```csharp
public static readonly HttpClient ApiClient = new HttpClient { BaseAddress = new Uri("https://localhost:7053/") };
```

กำหนด API base URL สำหรับฝั่งร้าน

จุดที่ควรระวังตอน demo: ฝั่งลูกค้าใช้ `http://localhost:5000/` แต่ฝั่งร้านใช้ `https://localhost:7053/` ต้องรัน API ให้ตรงกับ port ที่ตั้งไว้ ไม่อย่างนั้นต่อ API ไม่ได้

```csharp
public static int LoggedInRestaurantId = 0;
public static string LoggedInRestaurantName = "";
```

เก็บร้านที่ login อยู่ เพื่อให้ dashboard โหลดเฉพาะคิวของร้านนั้น

## 17. RestaurantLoginForm

เหมือน login ลูกค้า แต่ดึงข้อมูลร้านจาก `api/Restaurants`

```csharp
cmbRestaurants.DisplayMember = "Name";
cmbRestaurants.ValueMember = "RestaurantId";
```

แสดงชื่อร้าน แต่เก็บ id เพื่อใช้โหลด booking และ promotion

```csharp
Program.LoggedInRestaurantId = selectedRest.RestaurantId;
Program.LoggedInRestaurantName = selectedRest.Name;
```

จำร้านที่กำลังใช้งาน

## 18. RestaurantDashboardForm

เป็นหน้าหลักของฝั่งร้าน ใช้จัดการคิว

### Layout

```csharp
RestaurantUiTheme.StyleTabs(tabControl1);
```

ใช้ tab แยกคิวเป็น 2 กลุ่ม

- รอการยืนยัน
- คิวที่รับแล้ว

เหตุผลที่ใช้ tab เพราะลดความสับสน ร้านจะเห็นงานที่ต้องตัดสินใจก่อน และงานที่ต้องจัดการหลังยืนยันแยกกันชัดเจน

### LoadBookings

```csharp
api/Bookings/restaurant/{Program.LoggedInRestaurantId}
```

ดึง booking เฉพาะร้านที่ login อยู่

```csharp
var pendingBookings = allBookings.Where(b => b.Status == "Pending").ToList();
var confirmedBookings = allBookings.Where(b => b.Status == "Confirmed").ToList();
```

กรองข้อมูลตามสถานะ แล้วใส่คนละตาราง

### UpdateStatus

```csharp
var content = new StringContent(JsonSerializer.Serialize(newStatus), Encoding.UTF8, "application/json");
var response = await Program.ApiClient.PutAsync($"api/Bookings/{bookingId}/status", content);
```

ส่งสถานะใหม่ไปให้ API

เหตุผลที่ทำเป็น function กลาง เพราะปุ่ม Confirm, Reject, Complete, NoShow ทำงานเหมือนกันคือเปลี่ยน status ต่างกันแค่ค่าที่ส่ง

### ปุ่มต่าง ๆ

- `btnConfirm_Click` เปลี่ยนเป็น `Confirmed`
- `btnReject_Click` เปลี่ยนเป็น `Rejected`
- `btnComplete_Click` เปลี่ยนเป็น `Completed`
- `btnNoShow_Click` เปลี่ยนเป็น `NoShow`

สำหรับ NoShow มี confirm dialog เพราะเป็น action ที่กระทบประวัติลูกค้า

## 19. PromotionManagementForm

หน้าจัดการโปรโมชันของร้าน

```csharp
dtpExpireDate.MinDate = DateTime.Now;
```

ห้ามตั้งวันหมดอายุย้อนหลัง

```csharp
if (string.IsNullOrWhiteSpace(txtTitle.Text))
```

ตรวจว่าต้องมีชื่อโปรโมชันก่อนสร้าง

```csharp
var newPromo = new
{
    restaurantId = Program.LoggedInRestaurantId,
    title = txtTitle.Text,
    description = txtDescription.Text,
    discount = (double)numDiscount.Value,
    expireDate = dtpExpireDate.Value.ToString("yyyy-MM-ddTHH:mm:ssZ"),
    isActive = true
};
```

สร้างข้อมูลโปรโมชันส่งไป API

```csharp
await LoadPromotions();
```

หลังสร้างสำเร็จ รีเฟรชตารางทันที เพื่อให้ร้านเห็นผลลัพธ์

## 20. RestaurantUiTheme

คล้าย `CustomerUiTheme` แต่ใช้กับฝั่งร้านอาหาร

ข้อดีของการแยก theme เป็นไฟล์กลาง

- ลดโค้ดซ้ำในแต่ละ form
- แก้สี ปุ่ม ตาราง ได้จากที่เดียว
- ทำให้ UI ทั้งโปรแกรมหน้าตาไปทางเดียวกัน
- ช่วยให้พรีเซนต์เรื่อง design ได้ชัดว่ามีระบบ ไม่ได้จัดทีละหน้ามั่ว ๆ

## 21. DTO ฝั่ง UI

ในฝั่ง UI มีโฟลเดอร์ `Models` เช่น `BookingDTO`, `RestaurantDTO`, `PromotionDTO`, `ReviewDTO`, `CustomerDTO`

DTO คือ class ที่ใช้รับส่งข้อมูลระหว่าง UI กับ API

เหตุผลที่ใช้ DTO

- UI ไม่ต้องอ้างอิง model ของ server โดยตรง
- ข้อมูลจาก JSON deserialize เข้า object ได้ง่าย
- อ่านโค้ดง่าย เช่น `booking.Restaurant?.Name`

## 22. เหตุผลที่ใช้ Async ใน UI

หลายจุดใน WinForms ใช้ `async/await` เช่น

```csharp
var response = await Program.ApiClient.GetAsync(...);
```

เหตุผลคือการเรียก API ใช้เวลา ถ้าไม่ใช้ async หน้าจออาจค้างระหว่างรอ server ตอบกลับ

แต่ใน API มีหลายจุดใช้ `.ToList()` และ `.SaveChanges()` แบบ sync เพราะเป็นโปรเจกต์ขนาดเล็ก ใช้ง่ายและอ่านง่ายสำหรับ demo

ถ้าอาจารย์ถามว่าปรับปรุงได้ไหม ตอบได้ว่า production จริงควรใช้ `ToListAsync()` และ `SaveChangesAsync()` เพื่อรองรับผู้ใช้จำนวนมากขึ้น

## 23. ปัญหาที่พบและวิธีแก้

### ปัญหา 1: เวลาและ timezone

ปัญหา: PostgreSQL กับ C# DateTime อาจตีความ timezone ไม่ตรงกัน

วิธีแก้: ตั้งค่า

```csharp
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
```

และตอนส่งวันที่จาก UI ใช้ format ที่ชัดเจน เช่น `yyyy-MM-ddTHH:mm:ss`

### ปัญหา 2: จองโต๊ะเกินความจุ

ปัญหา: ถ้าหลายคนจองช่วงเดียวกัน ร้านอาจรับเกินความจุ

วิธีแก้: ใน `PostBooking` มีการรวมจำนวนคนในช่วงเวลาใกล้กัน แล้วเทียบกับ `MaxCapacity`

### ปัญหา 3: จองนอกเวลาเปิดปิด

ปัญหา: ลูกค้าอาจเลือกเวลาที่ร้านปิด

วิธีแก้: API ตรวจ `OpenHours` ก่อนบันทึก booking และรองรับกรณีเปิดข้ามคืน

### ปัญหา 4: ข้อมูลในตารางอ่านยาก

ปัญหา: DataGridView ค่า default ดูยาก และข้อความรีวิวยาวล้น

วิธีแก้: สร้าง `UiTheme` กลางและเปิด wrap text ในคอลัมน์ความคิดเห็น

### ปัญหา 5: ข้อมูลลูกค้าค้างหลัง logout

ปัญหา: ถ้าใช้ static เก็บ login แล้วไม่ล้าง คนถัดไปอาจใช้ข้อมูลคนก่อน

วิธีแก้: ตอน logout เคลียร์ `LoggedInCustomerId`, `LoggedInCustomerName`, และ `ClaimedCoupons`

## 24. จุดที่ AI ช่วยได้

สามารถพูดได้ประมาณนี้

AI ช่วยในส่วนการจัดโครงสร้างคำอธิบาย ช่วยเสนอแนวทาง UI ให้ดูเป็นระบบ ช่วยตรวจ logic บางจุด เช่น การเช็กความจุ การแยกสถานะ booking และช่วยสรุปเอกสารประกอบ

แต่ส่วนที่เราต้องเข้าใจเองคือ flow ของระบบ, business rules, database relationship, API endpoint แต่ละตัว, และการ demo ว่าปุ่มแต่ละปุ่มไปเปลี่ยนข้อมูลตรงไหน เพราะถ้าเกิด bug ตอนพรีเซนต์ เราต้องแก้และอธิบายเองได้

## 25. คำถามที่อาจโดนถาม

### ทำไมต้องแยก API กับ UI?

เพื่อให้ UI ไม่แตะฐานข้อมูลโดยตรง และรวม business rules ไว้ที่ server จุดเดียว ถ้าต่อไปเปลี่ยนจาก WinForms เป็นเว็บ ก็ยังใช้ API เดิมได้

### ทำไม booking ใหม่เป็น Pending?

เพราะร้านควรมีสิทธิ์ยืนยันก่อนว่ารับลูกค้าได้จริงหรือไม่ ไม่ใช่ให้ลูกค้าจองแล้ว confirmed ทันที

### ทำไมต้องมี MaxCapacity?

เพื่อป้องกันการรับลูกค้าเกินจำนวนที่ร้านรองรับได้ และทำให้ระบบมี logic มากกว่าแค่บันทึกข้อมูล

### ทำไม late cancel คือ 2 ชั่วโมง?

เป็นกติกาที่ตั้งขึ้นเพื่อป้องกันการยกเลิกใกล้เวลามากเกินไป ร้านจะได้ไม่เสียโอกาสรับลูกค้าคนอื่น

### ทำไมใช้ Entity Framework?

เพราะช่วยให้ทำงานกับ database ผ่าน class C# ได้ อ่านง่าย ลด SQL ซ้ำ ๆ และรองรับ relation ผ่าน navigation property เช่น `Booking.Customer`

### ทำไมใช้ DTO ใน UI?

เพราะ UI รับข้อมูล JSON จาก API แล้ว map เป็น object ฝั่ง UI ทำให้แยก client กับ server ออกจากกันชัดเจน

### ถ้าต้องปรับปรุงต่อจะทำอะไร?

- เพิ่มระบบ username/password จริง
- บันทึกคูปองที่ลูกค้ากดรับลง database
- ใช้ async ใน API ให้ครบ
- เพิ่ม validation ฝั่ง API ให้ละเอียดขึ้น
- เพิ่มระบบแจ้งเตือนเมื่อร้านยืนยัน booking
- เพิ่ม test สำหรับ business rules เช่น โต๊ะเต็ม ยกเลิกช้า และเวลาเปิดปิดข้ามคืน

## 26. ลำดับ demo ที่แนะนำ

1. รัน API ก่อน และเปิด Swagger ให้เห็นว่า server ทำงาน
2. เปิด Customer UI แล้วเลือกชื่อลูกค้า
3. ค้นหาร้านอาหาร
4. ดับเบิลคลิกร้าน ดูรายละเอียด โปรโมชัน และรีวิว
5. กดรับคูปอง แล้วลองจองโต๊ะ
6. เปิด Restaurant UI แล้วเลือกร้านเดียวกัน
7. ดู booking ในแท็บรอการยืนยัน
8. กดยืนยัน
9. กลับไปฝั่งลูกค้า ดูสถานะ booking
10. ฝั่งร้านกด completed
11. ฝั่งลูกค้าเขียนรีวิว
12. กลับไปดูคะแนนเฉลี่ยร้านและรีวิวที่เพิ่มขึ้น

## 27. ประโยคสรุปตอนจบ

โปรเจกต์นี้ออกแบบให้มี API เป็นศูนย์กลาง เชื่อมกับ PostgreSQL ผ่าน Entity Framework และมี Windows Forms แยกเป็นฝั่งลูกค้ากับฝั่งร้านอาหาร จุดเด่นคือไม่ได้เป็นแค่ CRUD แต่มี business rules เช่น ตรวจเวลาเปิดร้าน ตรวจความจุร้าน จัดการสถานะ booking ยกเลิกช้า รีวิว และโปรโมชัน ทำให้ระบบใกล้เคียงการใช้งานจริงมากขึ้น

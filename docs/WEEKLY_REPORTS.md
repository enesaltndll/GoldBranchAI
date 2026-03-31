# 📅 GoldBranch AI — Haftalık Geliştirme Raporu

> **Proje:** GoldBranch AI — Yapay Zeka Destekli Akıllı Proje & Görev Yönetim Sistemi  
> **Geliştirici:** Enes Altındal  
> **Dönem:** 2025-2026 Bahar Dönemi  
> **Teknolojiler:** ASP.NET Core 8.0 MVC, EF Core, SQL Server, Google Gemini AI, Bootstrap 5

---

## 📌 Hafta 1 — Temel Altyapı & Kimlik Doğrulama Sistemi

**Tarih:** 24 Şubat – 2 Mart 2026

### 🎯 Hedefler
- Projenin genel mimarisini belirlemek
- Kullanıcı kimlik doğrulama ve yetkilendirme altyapısını kurmak
- Veritabanı şemasının ilk versiyonunu oluşturmak

### ✅ Yapılan İşler

#### 1.1 Proje İskeleti & Mimari Kararlar
- ASP.NET Core 8.0 MVC projesi oluşturuldu
- `Program.cs` üzerinden Dependency Injection, middleware pipeline ve servis konfigürasyonları yapıldı
- Entity Framework Core 8.0 + SQL Server LocalDB bağlantısı kuruldu (Code-First yaklaşımı)
- Proje klasör yapısı belirlendi: `Controllers/`, `Models/`, `Views/`, `Services/`, `Data/`

#### 1.2 Veritabanı Modelleri
- **AppUser** modeli oluşturuldu: `Id`, `FullName`, `Email`, `Password`, `Role`, `CreatedAt`
- 3 farklı rol tanımlandı: `Admin`, `Proje Sefi`, `Gelistirici`
- **AppDbContext** oluşturuldu ve `DbSet<AppUser>` tanımlandı
- İlk migration oluşturuldu ve veritabanı ayağa kaldırıldı

#### 1.3 Kimlik Doğrulama (Authentication)
- Cookie-Based Authentication sistemi kuruldu
  - `CookieAuthenticationDefaults` ile oturum yönetimi
  - Login/Logout mekanizması
  - Claims-based yetkilendirme (`ClaimTypes.Email`, `ClaimTypes.Role`)
- **AuthController** oluşturuldu:
  - `Login()` — Email/şifre ile giriş
  - `Register()` — Yeni kullanıcı kaydı
  - `Logout()` — Oturum sonlandırma
- Google OAuth 2.0 entegrasyonu başlatıldı
  - `Microsoft.AspNetCore.Authentication.Google` NuGet paketi eklendi
  - Google Cloud Console üzerinde OAuth Client ID oluşturuldu
  - Harici giriş callback mekanizması hazırlandı

#### 1.4 Giriş & Kayıt Sayfaları
- `Login.cshtml` ve `Register.cshtml` tasarlandı
- Premium karanlık tema ile glassmorphism kartlar
- Google ile giriş butonu eklendi
- Form validation ve hata mesajları

### 📊 Çıktılar
| Dosya | Açıklama |
|-------|----------|
| `Program.cs` | DI, Auth, EF Core, Middleware konfigürasyonu |
| `Models/AppUser.cs` | Kullanıcı veri modeli |
| `Data/AppDbContext.cs` | Veritabanı bağlam sınıfı |
| `Controllers/AuthController.cs` | Kimlik doğrulama controller |
| `Views/Auth/Login.cshtml` | Giriş sayfası |
| `Views/Auth/Register.cshtml` | Kayıt sayfası |

### 🧪 Test & Doğrulama
- Admin, Proje Şefi ve Geliştirici rolleriyle başarılı giriş testi
- Google OAuth ile giriş akışı testi
- Veritabanında kullanıcı kayıtlarının oluştuğu doğrulandı

---

## 📌 Hafta 2 — Görev Yönetim Sistemi & Kanban Panosu

**Tarih:** 3 Mart – 9 Mart 2026

### 🎯 Hedefler
- Görev CRUD (Oluşturma, Okuma, Güncelleme, Silme) işlemlerini tamamlamak
- Kanban tarzı görsel görev panosu oluşturmak
- Rol bazlı iş akışını (workflow) kurmak

### ✅ Yapılan İşler

#### 2.1 TodoTask Modeli
- **TodoTask** modeli oluşturuldu:
  - `Title`, `Description`, `DueDate`, `Status`, `IsCompleted`
  - `EstimatedTimeHours`, `SpentTimeMinutes`, `AttachedFilePath`
  - `AppUserId` (FK) ile kullanıcıya bağlantı
- **Aura Logic** algoritması geliştirildi (NotMapped hesaplanan alanlar):
  - `AuraColor` — Görevin aciliyet rengi (danger/warning/success)
  - `UrgencyScore` — Sıralama için sayısal puan
  - Formül: Teslim tarihine kalan saat bazında otomatik kategorizasyon

#### 2.2 TaskController
- **TaskController** oluşturuldu:
  - `Index()` — Rol bazlı görev listesi ve Kanban panosu
  - `Create()` — Yeni görev oluşturma (sadece Proje Şefi)
  - `Edit()` — Görev düzenleme
  - `Details()` — Görev detay görüntüleme
  - `Delete()` — Görev silme (onay mekanizmasıyla)
  - `ChangeStatus()` — Statü değiştirme (iş akışı)
  - `Dashboard()` — Özet istatistikler

#### 2.3 4 Aşamalı İş Akışı
```
Devam Ediyor → [Geliştirici: Onaya Gönder] → Onay Bekliyor 
                                                    ↓
                                    [Şef: Onayla] → Tamamlandı
                                    [Şef: Reddet] → Revize → [Geliştirici tekrar gönderir]
```

#### 2.4 Kanban Board UI
- 4 şeritli Kanban panosu tasarlandı:
  - 🔴 **Kritik / Gecikenler** — `DueDate < Now`
  - 🟡 **Bu Hafta** — `Now ≤ DueDate ≤ Now+7`
  - 🔵 **Gelecek Hafta** — `Now+7 < DueDate ≤ Now+14`
  - ⚫ **İleri Tarihli** — `DueDate > Now+14`
- Yatay kaydırma (horizontal scroll) desteği
- Kompakt görev kartları + hover animasyonları

#### 2.5 Dosya Yükleme & Önizleme
- Görev oluşturma formuna dosya yükleme alanı eklendi
- `/wwwroot/uploads/` klasörüne kayıt
- PDF ve resim dosyaları için önizleme sistemi
- Dosya indirme bağlantısı

#### 2.6 Ekran Süresi Takibi
- **TimeTrackerController** oluşturuldu
- **DailyTimeLog** modeli eklendi
- Her 60 saniyede bir AJAX ping ile sunucuya süre bildirimi
- localStorage + sunucu senkronizasyonu
- Navbar'da canlı sayaç gösterimi (JetBrains Mono font)
- 6+ saat turuncu, 8+ saat kırmızı uyarı

### 📊 Çıktılar
| Dosya | Açıklama |
|-------|----------|
| `Models/TodoTask.cs` | Görev modeli + Aura Logic |
| `Models/DailyTimeLog.cs` | Günlük mesai log modeli |
| `Controllers/TaskController.cs` | Görev CRUD + iş akışı |
| `Controllers/TimeTrackerController.cs` | Süre takip API |
| `Views/Task/Index.cshtml` | Kanban board |
| `Views/Task/Create.cshtml` | Görev oluşturma formu |
| `Views/Task/Details.cshtml` | Görev detay sayfası |
| `Views/Task/Edit.cshtml` | Görev düzenleme formu |
| `Views/Task/Dashboard.cshtml` | Özet dashboard |

### 🧪 Test & Doğrulama
- Proje Şefi ile görev oluşturma, atama ve onaylama akışı test edildi
- Geliştirici ile "Onaya Gönder" ve "Revize" döngüsü test edildi
- Dosya yükleme ve PDF önizleme başarıyla çalıştırıldı
- Ekran süresi sayacının doğru çalıştığı doğrulandı

---

## 📌 Hafta 3 — Yapay Zeka Entegrasyonu (Google Gemini AI)

**Tarih:** 10 Mart – 16 Mart 2026

### 🎯 Hedefler
- Google Gemini AI API entegrasyonunu tamamlamak
- AI Görev Kırılımı modülünü geliştirmek
- Geliştirici AI Araştırma Asistanı'nı kurmak

### ✅ Yapılan İşler

#### 3.1 GeminiService
- **GeminiService** sınıfı oluşturuldu (`Services/GeminiService.cs`)
- Google Gemini 2.0 Flash modeli kullanıldı
- `HttpClient` ile RESTful API iletişimi
- JSON response parsing ve hata yönetimi
- Türkçe prompt mühendisliği (system instructions)
- API key güvenliği: `appsettings.json` üzerinden yapılandırma

#### 3.2 AI Görev Kırılımı (Breakdown)
- **AiController.Breakdown()** endpoint'i oluşturuldu
- Proje Şefi veya Admin bir görev tanımı girer
- Gemini AI, girilen metni analiz ederek:
  - Alt görevlere böler
  - Her alt görev için tahmini süre belirler
  - Öncelik seviyesi atar (Yüksek/Orta/Düşük)
  - Teknik notlar ve bağımlılıklar önerir
- **AiTaskBreakdown** modeli ile sonuçlar veritabanına kaydedilir
- Sonuçlar JSON formatında ayrıştırılıp güzel bir UI'da gösterilir
- `Views/Ai/Breakdown.cshtml` — Premium terminal tarzı arayüz

#### 3.3 AI Araştırma Asistanı (Research)
- **AiController.Research()** endpoint'i oluşturuldu
- Geliştirici rolüne özel erişim
- Teknik sorular sorulabilir (ör. "Entity Framework'te lazy loading nasıl çalışır?")
- Gemini AI Türkçe cevaplar üretir:
  - Detaylı açıklamalar
  - Kod örnekleri
  - Best practice önerileri
- **AiResearchLog** modeli ile araştırma geçmişi kaydedilir
- `Views/Ai/Research.cshtml` — Chat tarzı araştırma arayüzü

#### 3.4 Prompt Mühendisliği
- Görev kırılımı için özelleştirilmiş system prompt:
  ```
  "Sen bir yazılım proje yöneticisisin. Verilen iş tanımını alt görevlere böl.  
   Her alt görev için: başlık, açıklama, tahmini süre (saat), öncelik belirle.
   JSON formatında yanıt ver."
  ```
- Araştırma asistanı için özelleştirilmiş system prompt:
  ```
  "Sen bir kıdemli yazılım mühendisisin. Sorulan teknik soruya Türkçe,
   detaylı ve kod örnekleriyle cevap ver."
  ```

### 📊 Çıktılar
| Dosya | Açıklama |
|-------|----------|
| `Services/GeminiService.cs` | Gemini AI API servisi |
| `Controllers/AiController.cs` | AI endpoint'leri |
| `Models/AiTaskBreakdown.cs` | AI kırılım veri modeli |
| `Models/AiResearchLog.cs` | AI araştırma log modeli |
| `Views/Ai/Breakdown.cshtml` | Görev kırılım UI |
| `Views/Ai/Research.cshtml` | Araştırma asistanı UI |

### 🧪 Test & Doğrulama
- Farklı görev tanımlarıyla kırılım testi yapıldı
- JSON parsing doğruluğu kontrol edildi
- Araştırma asistanı ile çeşitli teknik sorular soruldu
- API hata durumları (ağ hatası, geçersiz key) için fallback mekanizması test edildi

---

## 📌 Hafta 4 — İletişim Sistemi & Admin Yönetim Araçları

**Tarih:** 17 Mart – 23 Mart 2026

### 🎯 Hedefler
- WhatsApp tarzı mesajlaşma altyapısını kurmak
- Grup sohbet özelliğini geliştirmek
- Admin yönetim araçlarını tamamlamak

### ✅ Yapılan İşler

#### 4.1 Mesajlaşma Altyapısı
- **ChatMessage** modeli oluşturuldu:
  - `SenderId`, `ReceiverId` — DM için
  - `ChatGroupId` — Grup mesajları için
  - `MessageText`, `SentAt`
  - Hibrit yapı: Aynı model hem DM hem grup mesajlarını destekler
- **ChatController** oluşturuldu:
  - `Index()` — WhatsApp tarzı ana ekran (sidebar + chat area)
  - `Room()` — iframe içi sohbet odası
  - `SendMessage()` — Mesaj gönderme (AJAX fetch API)
  - `CreateGroup()` — Grup oluşturma

#### 4.2 Rol Bazlı Mesajlaşma Kuralları
- **Admin:** Sadece Proje Şeflerine DM atabilir + Tüm konuşmaları izleyebilir
- **Proje Şefi:** Geliştiricilere DM atabilir
- **Geliştirici:** Proje Şeflerine DM atabilir
- Admin İzleme Modu: Sidebar'da "Sistem İzleme" kategorisi

#### 4.3 Grup Sohbet
- **ChatGroup** modeli: `GroupName`, `CreatedByUserId`, `CreatedAt`
- **ChatGroupMember** modeli: Çoktan-çoğa ilişki ara tablosu
- Fluent API ile `DeleteBehavior.Restrict` konfigürasyonu
  - SQL Server 1785 hatasını (Multiple Cascade Paths) önlemek için
- Grup oluşturma paneli: Ad girişi + üye seçimi (checkbox listesi)
- Grup mesajlarında renkli gönderici isimleri

#### 4.4 WhatsApp Tarzı UI
- Sol panel: Gruplar, DM'ler ve İzleme modu kategorileri
- Sağ panel: iframe tabanlı gerçek zamanlı sohbet
- Mesaj balonları: Giden (mavi), Gelen (koyu gri)
- Otomatik scroll-to-bottom
- Gönderildi tikleri (✓✓)
- Boş durum ekranı (WhatsApp benzeri)

#### 4.5 Admin Yönetim Paneli
- **AdminController** oluşturuldu:
  - `Users()` — Kullanıcı listesi, rol değiştirme, silme
  - `Roadmap()` — Proje yol haritası ve efor takibi
  - `WorkReports()` — Z-Raporu (görev analitikleri)
  - `Leaderboard()` — Geliştirici performans sıralaması
  - `BurnoutMap()` — Tükenmişlik ısı haritası
  - `SystemLogs()` — Sistem olay logları (terminal tarzı)
- Offcanvas sidebar ile hızlı erişim menüsü
- Her admin aracı için özel tasarım ve veri görselleştirmesi

### 📊 Çıktılar
| Dosya | Açıklama |
|-------|----------|
| `Models/ChatMessage.cs` | Mesaj modeli (DM + Grup) |
| `Models/ChatGroup.cs` | Grup yapısı |
| `Models/ChatGroupMember.cs` | Üyelik ara tablosu |
| `Models/SystemLog.cs` | Sistem log modeli |
| `Controllers/ChatController.cs` | Mesajlaşma controller |
| `Controllers/AdminController.cs` | Admin panel controller |
| `Views/Chat/Index.cshtml` | WhatsApp ana ekran |
| `Views/Chat/Room.cshtml` | Sohbet odası |
| `Views/Admin/Users.cshtml` | Kullanıcı yönetimi |
| `Views/Admin/Roadmap.cshtml` | Yol haritası |
| `Views/Admin/WorkReports.cshtml` | Z-Raporu |
| `Views/Admin/Leaderboard.cshtml` | Performans ligi |
| `Views/Admin/BurnoutMap.cshtml` | Isı haritası |
| `Views/Admin/SystemLogs.cshtml` | Sistem radarı |

### 🧪 Test & Doğrulama
- 3 farklı rolle DM gönderme/alma testi yapıldı
- Grup oluşturma ve grup mesajlaşma testi yapıldı
- Admin izleme modunda Geliştirici-Şef mesajlarının görüntülendiği doğrulandı
- Kullanıcı silme ve rol değiştirme işlemleri test edildi

---

## 📌 Hafta 5 — UI/UX Optimizasyonu, Tema Sistemi & Test

**Tarih:** 24 Mart – 31 Mart 2026

### 🎯 Hedefler
- Premium tasarım sistemini tamamlamak
- Tema değiştirme özelliğini eklemek
- Tüm sayfaları test edip hataları gidermek
- GitHub dokümantasyonunu hazırlamak

### ✅ Yapılan İşler

#### 5.1 Premium CSS Tasarım Sistemi (`site.css`)
- 840+ satır özelleştirilmiş CSS yazıldı
- CSS Custom Properties (değişkenler) ile merkezi renk ve spacing yönetimi
- Glassmorphism efektleri: Yarı saydam kartlar, blur arka planlar
- Neon glow efektleri: Altın ve cyan parıltılar
- Mikro-animasyonlar: `fadeInUp`, `fadeInScale`, `shimmer`, `float`, `glowPulse`
- Özel scrollbar tasarımı
- Responsive breakpoint'ler

#### 5.2 Çoklu Tema Sistemi
5 farklı tema geliştirildi (CSS Custom Properties ile):

| Tema | Ana Renk | Karakter |
|------|----------|----------|
| 🟡 Altın Karanlık | `#fbbf24` | Varsayılan, profesyonel |
| 🔵 Uzay Mavisi | `#38bdf8` | Soğuk, modern |
| 🟣 Neon Mor | `#c084fc` | Cyberpunk |
| 🟢 Matrix Yeşili | `#34d399` | Hacker |
| 🔴 Ateş Kırmızısı | `#f87171` | Agresif |

- `localStorage` ile tema tercihi kalıcı olarak saklanır
- Floating action button ile tema değiştirme offcanvas paneli
- Canlı önizleme efekti: Sayfa anında güncellenir

#### 5.3 Animasyonlu Parçacık Arka Planı
- HTML5 Canvas ile interaktif parçacık sistemi
- 60 parçacık: Altın, cyan ve mor renklerinde
- Parçacıklar arası mesafe bazlı bağlantı çizgileri
- Performans optimizasyonu: `requestAnimationFrame`
- `body::before` ve `body::after` ile gradient mesh arka plan

#### 5.4 Ana Layout Optimizasyonu (`_Layout.cshtml`)
- Sticky navbar: scroll'da sabit kalma
- Admin offcanvas sidebar: Hover efektli menü öğeleri
- Tema değiştirme offcanvas: Sağdan açılan panel
- Canlı ekran süresi sayacı: Navbar'da JetBrains Mono font ile
- Rol bazlı menü görünürlüğü

#### 5.5 Hata Giderme & Optimizasyon
- **EF Core sorgu hatası düzeltildi:** `.Include()` metodu `.Select()`'ten sonra çağrılamıyordu, sorgu yeniden yapılandırıldı
- **Modal siyah ekran sorunu çözüldü:** Bootstrap modal backdrop'u Layout'un z-index katmanlarıyla çakışıyordu. Tüm modallar Bootstrap modal yerine custom CSS overlay panelleriyle değiştirildi
- **SQL Server 1785 hatası çözüldü:** Çoklu cascade path hatası, Fluent API ile `DeleteBehavior.Restrict` kullanılarak giderildi
- **Chat Room CSS düzeltmesi:** Yerel Bootstrap yolu CDN ile değiştirildi
- **Dosya önizleme scroll sorunu:** embed yerine iframe kullanılarak çözüldü

#### 5.6 GitHub Dokümantasyonu
- Kapsamlı `README.md` hazırlandı
  - Proje tanıtımı ve özellikler
  - Mimari diyagramlar (ASCII art)
  - Veritabanı şeması
  - Rol-yetkilendirme matrisi
  - Kurulum kılavuzu
  - Proje yapısı ağacı
- `docs/WEEKLY_REPORTS.md` — 5 haftalık detaylı geliştirme raporu

### 📊 Çıktılar
| Dosya | Açıklama |
|-------|----------|
| `wwwroot/css/site.css` | 840+ satır premium CSS (5 tema) |
| `Views/Shared/_Layout.cshtml` | Ana layout (navbar, sidebar, canvas) |
| `README.md` | GitHub README dokümantasyonu |
| `docs/WEEKLY_REPORTS.md` | Haftalık geliştirme raporları |

### 🧪 Test & Doğrulama
- 3 farklı rolle tüm sayfa akışları test edildi
- 5 tema arasında geçiş yapılarak görsel tutarlılık kontrol edildi
- Chat, Kanban, AI modülleri uçtan uca test edildi
- Veritabanı CRUD işlemleri doğrulandı
- Build: 0 hata, 25 uyarı (nullable reference — bloklamayan)

---

## 📈 Toplam Proje İstatistikleri

| Metrik | Değer |
|--------|-------|
| **Toplam Controller** | 7 |
| **Toplam Model** | 10 |
| **Toplam View** | 18+ |
| **Toplam CSS Satırı** | 840+ |
| **Veritabanı Tablosu** | 9 |
| **Kullanıcı Rolleri** | 3 |
| **Tema Seçeneği** | 5 |
| **AI Modülü** | 2 (Kırılım + Araştırma) |
| **3. Parti API** | Google Gemini AI, Google OAuth 2.0 |
| **NuGet Paketleri** | EF Core, Google Auth |

---

## 🔮 Gelecek Planları (İleri Geliştirme)

- [ ] SignalR ile gerçek zamanlı mesajlaşma (WebSocket)
- [ ] Bildirim sistemi (push notifications)
- [ ] Görev sürükleme ile Kanban lane değiştirme (drag & drop)
- [ ] AI ile otomatik görev atama önerisi
- [ ] Mobil responsive optimizasyon (PWA)
- [ ] Docker ile containerization
- [ ] CI/CD pipeline (GitHub Actions)
- [ ] Birim testleri (xUnit)

---

*Bu rapor, GoldBranch AI projesinin 5 haftalık geliştirme sürecini detaylı olarak belgelemektedir.*

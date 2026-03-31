<p align="center">
  <img src="https://img.shields.io/badge/.NET-8.0-blueviolet?style=for-the-badge&logo=dotnet" />
  <img src="https://img.shields.io/badge/EF_Core-8.0-blue?style=for-the-badge&logo=nuget" />
  <img src="https://img.shields.io/badge/SQL_Server-LocalDB-red?style=for-the-badge&logo=microsoftsqlserver" />
  <img src="https://img.shields.io/badge/Gemini_AI-Integrated-green?style=for-the-badge&logo=google" />
  <img src="https://img.shields.io/badge/Bootstrap-5.3-purple?style=for-the-badge&logo=bootstrap" />
</p>

<h1 align="center">🌿 GoldBranch AI</h1>
<h3 align="center">Yapay Zeka Destekli Akıllı Proje & Görev Yönetim Sistemi</h3>

<p align="center">
  <i>Ekip yönetimi, görev takibi, yapay zeka iş analizi ve gerçek zamanlı iletişimi tek bir platformda birleştiren kurumsal düzeyde bir web uygulaması.</i>
</p>

---

## 📋 İçindekiler

- [Proje Hakkında](#-proje-hakkında)
- [Öne Çıkan Özellikler](#-öne-çıkan-özellikler)
- [Mimari & Teknolojiler](#-mimari--teknolojiler)
- [Veritabanı Şeması](#-veritabanı-şeması)
- [Rol Tabanlı Yetkilendirme](#-rol-tabanlı-yetkilendirme)
- [Ekran Görüntüleri](#-ekran-görüntüleri)
- [Kurulum & Çalıştırma](#-kurulum--çalıştırma)
- [Proje Yapısı](#-proje-yapısı)
- [Geliştirme Süreci](#-geliştirme-süreci)

---

## 🎯 Proje Hakkında

**GoldBranch AI**, yazılım geliştirme ekiplerinin proje yönetim süreçlerini dijitalleştirmek ve yapay zeka ile desteklemek amacıyla geliştirilmiş kapsamlı bir ASP.NET Core MVC web uygulamasıdır.

Sistem, üç farklı rol (Admin, Proje Şefi, Geliştirici) üzerinden çalışır ve her role özel paneller, yetkiler ve iş akışları sunar. Google Gemini AI entegrasyonu sayesinde görev alt kırılımları, araştırma önerileri ve zaman tahminleri otomatik olarak üretilebilir.

### Projenin Çözdüğü Problemler

| Problem | GoldBranch AI Çözümü |
|---------|---------------------|
| Görev dağılımı ve takibi zor | Kanban panosu + Akıllı sıralama (Aura Logic) |
| Ekip içi iletişim dağınık | WhatsApp tarzı entegre mesajlaşma sistemi |
| İş yükü analizi yapılamıyor | Isı haritası + Tükenmişlik analizi |
| Toplantılarda zaman kaybı | AI ile otomatik görev alt kırılımı |
| Performans ölçülemiyor | Liderlik tablosu + Z-Raporu |
| Mesai takibi yok | Otomatik ekran süresi sayacı |

---

## ✨ Öne Çıkan Özellikler

### 🧠 Yapay Zeka Modülleri (Google Gemini AI)
- **AI Görev Kırılımı** — Proje Şefi bir iş tanımı girer, Gemini AI bunu alt görevlere böler, tahmini süre ve öncelik atar
- **AI Araştırma Asistanı** — Geliştiriciler teknik sorular sorar, AI Türkçe olarak detaylı cevaplar ve kod örnekleri üretir
- **Akıllı Sıralama (Aura Logic)** — Görevleri teslim tarihine göre otomatik renk kodlaması: 🔴Acil / 🟡Yaklaşan / 🟢Rahat

### 📋 Görev Yönetimi (Kanban Board)
- Sürüklenebilir olmayan ama zaman bazlı otomatik kategorize olan Kanban panosu
- **Kritik/Gecikenler → Bu Hafta → Gelecek Hafta → İleri Tarihli** şeritleri
- 4 aşamalı iş akışı: `Devam Ediyor → Onay Bekliyor → Revize / Tamamlandı`
- Dosya ekleme ve önizleme desteği (PDF, resim)

### 💬 WhatsApp Tarzı Mesajlaşma
- **Direkt Mesajlar (DM):** Rol bazlı kısıtlı mesajlaşma
- **Grup Sohbetleri:** Admin ve Proje Şefi grup oluşturabilir, üye seçebilir
- **Admin İzleme Modu:** Admin, Geliştirici-Şef arası tüm konuşmaları görebilir
- iframe tabanlı gerçek zamanlı sohbet penceresi

### 👥 Rol Tabanlı Yetkilendirme
- Cookie-Based Authentication + Google OAuth 2.0
- 3 farklı rol: Admin, Proje Şefi, Geliştirici
- Her rol için ayrı dashboard, menü ve yetkiler

### 📊 Admin Yönetim Araçları
- **Yol Haritası:** Proje aşamaları ve efor takibi
- **Ekip Yönetimi:** Kullanıcı ekleme/silme, rol değiştirme
- **Z-Raporu Odası:** Görev analitik verileri
- **Performans Ligi:** Geliştirici sıralaması
- **Isı Haritası (Burnout Map):** Tükenmişlik analizi
- **Sistem Radarı:** Gerçek zamanlı log takibi

### 🎨 Premium UI/UX Tasarım
- 5 farklı tema: Altın Karanlık, Uzay Mavisi, Neon Mor, Matrix Yeşili, Ateş Kırmızısı
- Glassmorphism + Neon Glow efektleri
- Animasyonlu parçacık arka planı (Particle Canvas)
- JetBrains Mono + Inter font ailesi
- Tamamen responsive tasarım

---

## 🏗 Mimari & Teknolojiler

```
┌──────────────────────────────────────────────────┐
│                   SUNUM KATMANI                   │
│  Razor Views (.cshtml) + Bootstrap 5.3 + CSS3    │
│  Glassmorphism / Particle Effects / Animations    │
├──────────────────────────────────────────────────┤
│                  İŞ MANTIK KATMANI                │
│  ASP.NET Core 8.0 MVC Controllers               │
│  TaskController, ChatController, AdminController  │
│  AiController, AuthController, TimeTracker       │
├──────────────────────────────────────────────────┤
│                 SERVİS KATMANI                    │
│  GeminiService (Google Gemini AI API)            │
│  Cookie Authentication + Google OAuth 2.0        │
├──────────────────────────────────────────────────┤
│               VERİ ERİŞİM KATMANI                │
│  Entity Framework Core 8.0 (Code-First)          │
│  SQL Server LocalDB                              │
│  AppDbContext + Fluent API Konfigürasyonu         │
└──────────────────────────────────────────────────┘
```

| Teknoloji | Sürüm | Kullanım Amacı |
|-----------|-------|----------------|
| ASP.NET Core MVC | 8.0 | Web framework |
| Entity Framework Core | 8.0 | ORM, veritabanı yönetimi |
| SQL Server LocalDB | 2019+ | İlişkisel veritabanı |
| Google Gemini AI | 2.0 Flash | Yapay zeka servisleri |
| Bootstrap | 5.3.2 | UI bileşenleri |
| Font Awesome | 6.4.2 | İkonlar |
| Google OAuth 2.0 | — | Sosyal giriş |
| HTML5 Canvas | — | Parçacık animasyonları |

---

## 🗃 Veritabanı Şeması

```
┌───────────────┐     ┌───────────────┐     ┌───────────────┐
│   AppUser     │     │   TodoTask    │     │  ChatMessage  │
├───────────────┤     ├───────────────┤     ├───────────────┤
│ Id (PK)       │────▶│ AppUserId(FK) │     │ SenderId (FK) │
│ FullName      │     │ Title         │     │ ReceiverId(FK)│
│ Email         │     │ Description   │     │ ChatGroupId   │
│ Password      │     │ DueDate       │     │ MessageText   │
│ Role          │     │ Status        │     │ SentAt        │
│ CreatedAt     │     │ IsCompleted   │     └───────────────┘
└───────────────┘     │ EstimatedTime │            │
        │             │ SpentTime     │            │
        │             │ AttachedFile  │     ┌──────┴────────┐
        │             │ AuraColor*    │     │  ChatGroup    │
        │             │ UrgencyScore* │     ├───────────────┤
        │             └───────────────┘     │ Id (PK)       │
        │                                   │ GroupName      │
        ▼                                   │ CreatedByUser  │
┌───────────────┐     ┌───────────────┐     │ CreatedAt      │
│ DailyTimeLog  │     │ChatGroupMember│     └───────────────┘
├───────────────┤     ├───────────────┤
│ AppUserId(FK) │     │ ChatGroupId   │
│ LogDate       │     │ AppUserId     │
│ TotalMinutes  │     └───────────────┘
└───────────────┘
                      * = NotMapped (hesaplanan alan)

┌───────────────┐     ┌───────────────┐
│  SystemLog    │     │AiTaskBreakdown│
├───────────────┤     ├───────────────┤
│ ActionType    │     │ TaskTitle     │
│ Message       │     │ SubTasks(JSON)│
│ CreatedAt     │     │ CreatedAt     │
└───────────────┘     └───────────────┘
```

### Fluent API Konfigürasyonu
- Tüm `ChatMessage` ilişkileri `DeleteBehavior.Restrict` — SQL Server çoklu cascade path hatası önlemi
- `ChatGroupMember` çoktan-çoğa ara tablo yapısı

---

## 🔐 Rol Tabanlı Yetkilendirme

| Özellik | 👑 Admin | ⭐ Proje Şefi | 💻 Geliştirici |
|---------|----------|--------------|---------------|
| Dashboard Görüntüleme | ✅ Tüm sistem | ✅ Kendi ekibi | ✅ Kendi görevleri |
| Görev Oluşturma | ❌ | ✅ | ❌ |
| Görev Onaylama/Reddetme | ❌ | ✅ | ❌ |
| Görev Tamamlama Bildirimi | ❌ | ❌ | ✅ |
| AI Görev Kırılımı | ✅ | ✅ | ❌ |
| AI Araştırma Asistanı | ❌ | ❌ | ✅ |
| Grup Sohbet Oluşturma | ✅ | ✅ | ❌ |
| DM Gönderme | Sadece Şeflere | Geliştiricilere | Şeflere |
| Mesaj İzleme | ✅ Tüm mesajlar | ❌ | ❌ |
| Kullanıcı Yönetimi | ✅ | ❌ | ❌ |
| Performans Ligi | ✅ | ❌ | ❌ |
| Isı Haritası | ✅ | ❌ | ❌ |
| Tema Değiştirme | ✅ | ✅ | ✅ |
| Ekran Süresi Takibi | ✅ | ✅ | ✅ |

---

## 📸 Ekran Görüntüleri

> Uygulama 5 farklı tema ile kullanılabilir. Aşağıdaki ekran görüntüleri varsayılan "Altın Karanlık" temasındadır.

### Ana Dashboard
Kartlarla proje özeti, grafikler ve hızlı erişim panelleri.

### Kanban Görev Panosu
Görevler zaman bazlı otomatik kategorilerde: Kritik, Bu Hafta, Gelecek Hafta, İleri Tarihli.

### WhatsApp Tarzı Mesajlaşma
Sol panel: Gruplar, DM'ler ve Admin izleme modu. Sağ panel: iframe tabanlı sohbet penceresi.

### AI Görev Kırılımı (Gemini AI)
Proje Şefi bir iş tanımı girer, AI otomatik alt görevlere böler.

### Admin Komuta Paneli
Offcanvas sidebar ile tüm yönetim araçlarına hızlı erişim.

---

## 🚀 Kurulum & Çalıştırma

### Ön Koşullar
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server LocalDB](https://docs.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb) veya SQL Server Express
- [Visual Studio 2022](https://visualstudio.microsoft.com/) (Opsiyonel, VS Code ile de çalışır)

### Adımlar

```bash
# 1. Repoyu klonlayın
git clone https://github.com/enesaltndll/GoldBranchAI.git
cd GoldBranchAI

# 2. Bağımlılıkları yükleyin
dotnet restore

# 3. Veritabanını oluşturun
# Not: Uygulama ilk çalıştırmada veritabanını otomatik oluşturur (EnsureCreated)

# 4. Uygulamayı çalıştırın
dotnet run
```

### Varsayılan Giriş Bilgileri

| Rol | E-posta | Şifre |
|-----|---------|-------|
| 👑 Admin | admin@goldbranch.com | admin123 |
| ⭐ Proje Şefi | sef@goldbranch.com | sef123 |
| 💻 Geliştirici | dev@goldbranch.com | dev123 |

### `appsettings.json` Yapılandırması

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=GoldBranchAI;..."
  },
  "GeminiApi": {
    "ApiKey": "YOUR_GEMINI_API_KEY"
  },
  "Authentication": {
    "Google": {
      "ClientId": "YOUR_GOOGLE_CLIENT_ID",
      "ClientSecret": "YOUR_GOOGLE_CLIENT_SECRET"
    }
  }
}
```

> ⚠️ Google Gemini API anahtarı almak için [Google AI Studio](https://makersuite.google.com/app/apikey) adresini ziyaret edin.

---

## 📁 Proje Yapısı

```
GoldBranchAI/
├── Controllers/
│   ├── AdminController.cs      # Admin yönetim paneli işlemleri
│   ├── AiController.cs         # Gemini AI entegrasyon endpointleri
│   ├── AuthController.cs       # Giriş/Kayıt/Google OAuth
│   ├── ChatController.cs       # Mesajlaşma ve grup yönetimi
│   ├── TaskController.cs       # Görev CRUD ve iş akışı
│   └── TimeTrackerController.cs # Ekran süresi takibi
│
├── Models/
│   ├── AppUser.cs              # Kullanıcı modeli (3 rol)
│   ├── TodoTask.cs             # Görev modeli (Aura Logic dahil)
│   ├── ChatMessage.cs          # DM ve grup mesajları
│   ├── ChatGroup.cs            # Grup sohbet yapısı
│   ├── ChatGroupMember.cs      # Grup üyelik ara tablosu
│   ├── DailyTimeLog.cs         # Günlük mesai kaydı
│   ├── SystemLog.cs            # Sistem olay logları
│   ├── AiTaskBreakdown.cs      # AI görev kırılım kaydı
│   └── AiResearchLog.cs        # AI araştırma kaydı
│
├── Views/
│   ├── Admin/                  # Yönetim paneli sayfaları
│   │   ├── BurnoutMap.cshtml   # Tükenmişlik ısı haritası
│   │   ├── Leaderboard.cshtml  # Performans ligi
│   │   ├── Roadmap.cshtml      # Proje yol haritası
│   │   ├── SystemLogs.cshtml   # Sistem radar logları
│   │   ├── Users.cshtml        # Kullanıcı yönetimi
│   │   └── WorkReports.cshtml  # Z-Raporu odası
│   │
│   ├── Ai/
│   │   ├── Breakdown.cshtml    # AI görev kırılım paneli
│   │   └── Research.cshtml     # AI araştırma asistanı
│   │
│   ├── Auth/
│   │   ├── Login.cshtml        # Giriş sayfası
│   │   └── Register.cshtml     # Kayıt sayfası
│   │
│   ├── Chat/
│   │   ├── Index.cshtml        # WhatsApp tarzı ana ekran
│   │   └── Room.cshtml         # Sohbet odası (iframe)
│   │
│   ├── Task/
│   │   ├── Index.cshtml        # Kanban görev panosu
│   │   ├── Create.cshtml       # Yeni görev oluşturma
│   │   ├── Details.cshtml      # Görev detay sayfası
│   │   ├── Edit.cshtml         # Görev düzenleme
│   │   └── Dashboard.cshtml    # Özet dashboard
│   │
│   └── Shared/
│       └── _Layout.cshtml      # Ana layout (Navbar, Tema, Admin Sidebar)
│
├── Services/
│   └── GeminiService.cs        # Google Gemini AI API servisi
│
├── Data/
│   └── AppDbContext.cs          # EF Core DbContext + Fluent API
│
├── wwwroot/
│   └── css/
│       └── site.css             # Premium tema CSS (5 tema, animasyonlar)
│
├── Program.cs                   # DI, Middleware, Auth, DB konfigürasyonu
├── appsettings.json             # Bağlantı dizgisi, API anahtarları
└── docs/
    └── WEEKLY_REPORTS.md        # 5 haftalık geliştirme raporu
```

---

## 📅 Geliştirme Süreci

Projenin 5 haftalık geliştirme süreci detaylı rapor olarak [`docs/WEEKLY_REPORTS.md`](docs/WEEKLY_REPORTS.md) dosyasında bulunmaktadır.

| Hafta | Odak Noktası | Öne Çıkan Çıktılar |
|-------|-------------|---------------------|
| 1. Hafta | Temel Altyapı | Proje iskeleti, Auth sistemi, DB şeması |
| 2. Hafta | Görev Yönetimi | CRUD, Kanban, iş akışı, dosya yükleme |
| 3. Hafta | AI Entegrasyonu | Gemini AI, görev kırılımı, araştırma asistanı |
| 4. Hafta | İletişim & Admin | Mesajlaşma, grup sohbet, admin araçları |
| 5. Hafta | UI/UX & Optimizasyon | 5 tema, animasyonlar, performans, test |

---

## 👨‍💻 Geliştirici

**Enes Altındal**

---

## 📄 Lisans

Bu proje eğitim amaçlı geliştirilmiştir.

---

<p align="center">
  <b>⭐ Projeyi beğendiyseniz yıldız vermeyi unutmayın! ⭐</b>
</p>

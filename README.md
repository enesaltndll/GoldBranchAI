<p align="center">
  <img src="https://img.shields.io/badge/.NET-8.0-blueviolet?style=for-the-badge&logo=dotnet" />
  <img src="https://img.shields.io/badge/EF_Core-8.0-blue?style=for-the-badge&logo=nuget" />
  <img src="https://img.shields.io/badge/SQL_Server-LocalDB-red?style=for-the-badge&logo=microsoftsqlserver" />
  <img src="https://img.shields.io/badge/SignalR-Realtime-orange?style=for-the-badge&logo=signalr" />
  <img src="https://img.shields.io/badge/Gemini_AI-Integrated-green?style=for-the-badge&logo=google" />
  <img src="https://img.shields.io/badge/Bootstrap-5.3-purple?style=for-the-badge&logo=bootstrap" />
</p>

<h1 align="center">🌿 GoldBranch AI</h1>
<h3 align="center">Yapay Zekâ Destekli ve Oyunlaştırma Tabanlı Akıllı Proje Yönetim Platformu</h3>

<p align="center">
  <i>Ekip koordinasyonu, akıllı görev yönetimi, yapay zekâ analizleri, sesli asistan desteği, SignalR canlı sohbet ve Jitsi video konferans entegrasyonunu tek bir fütüristik arayüzde birleştiren kurumsal düzeyde bir web uygulaması.</i>
</p>

---

## 📋 Proje Hakkında

**GoldBranch AI**, yazılım geliştirme süreçlerini daha verimli ve eğlenceli hale getirmek için klasik proje yönetim araçlarının (Kanban, To-Do listeleri) sınırlarını aşarak modern teknolojilerle donatılmış web tabanlı bir platformdur. 

Bu proje, **T.C. Sinop Üniversitesi Ayancık Meslek Yüksekokulu İnternet ve Ağ Teknolojileri Bölümü** kapsamında yürütülen **Sistem Analizi ve Tasarımı** dersi final çalışması olarak 10 haftalık bir süreçte baştan sona tasarlanıp kodlanmıştır.

---

## ✨ Öne Çıkan Ana Modüller

1. **📊 Çalışma Masası (Working Desk):** 
   Geliştiricilerin iş stresini ölçen **Sinirsel Yük Endeksi (Neural Load Index)**, akıllı sıralama yapan **Aciliyet Skoru (Urgency Score)** ve projenin geçmiş günlerdeki durumunu canlandıran **Zaman Makinesi (Project Time Machine)** entegrasyonu.
   
2. **📋 Sürükle-Bırak Kanban Tahtası:**
   Görevlerin durum geçişlerini anlık güncelleyen, bağımlı görevler tamamlanana kadar kartları kilitleyen **Görev Bağımlılık Kilidi (Task Lock)** ve tamamlanma olasılığını gösteren yapay zekâ hesaplayıcıları.

3. **🤖 Çoklu Yapay Zekâ (AI) Motoru:**
   Proje tanımlarından otomatik mikro görevler üreten **AI Breakdown**, geliştirici teknik araştırma asistanı, son mesajları analiz eden **Sohbet Özetleyici** ve akademik ödev üretici modülleri.

4. **🎙️ Sesli Asistan (Goldie HUD):**
   Tarayıcı tabanlı konuşma tanıma (Speech-to-Text) ve ses sentezleme (Text-to-Speech) teknolojisiyle sesli komutlarla görev oluşturma, sayfa yönlendirme ve sesli geri bildirim altyapısı.

5. **🎮 Zen Modu ve Oyunlaştırma:**
   Görev tamamlayarak XP kazanma, unvan ve başarı rozetleri edinme sistemi. Mola zamanlarında odaklanma sesleri ve dinlenmek için entegre yılan oyunu (Snake Game).

6. **💼 SaaS Abonelik & Payment:**
   Farklı üyelik planları (Free, Silver, Gold, Diamond), oyunlaştırma başarılarına göre dinamik indirim kodları tanımlayan akıllı sepet ve fatura ödeme simülasyonu.

7. **💬 İletişim ve Konferans:**
   SignalR altyapılı anlık bireysel ve grup sohbet sistemi ile Jitsi WebRTC görüntülü konferans odaları.

8. **🛡️ Yönetim Paneli & Burnout Map:**
   Geliştiricilerin haftalık çalışma sürelerini görselleştirip aşırı iş yükünde uyarı veren **Tükenmişlik Isı Haritası**, performans liderlik tabloları ve ClosedXML ile Excel veri export desteği.

---

## 🏗️ Mimari ve Teknolojik Katmanlar

```text
┌────────────────────────────────────────────────────────┐
│                     SUNUM KATMANI                      │
│   Razor (.cshtml) + Modern CSS (Custom Glassmorphism)  │
│   Vanilla JS (SignalR Client, Web Speech, Chart.js)    │
├────────────────────────────────────────────────────────┤
│                    İŞ MANTIK KATMANI                   │
│   ASP.NET Core 8.0 MVC / SignalR Hubs                  │
│   Gamification Engine, Neural Load Calculator          │
├────────────────────────────────────────────────────────┤
│                     SERVİS KATMANI                     │
│   Google Gemini API, Cohere & SambaNova/Llama Entegr.  │
│   Telegram Bot API, Discord Webhook, SMTP Mailer       │
├────────────────────────────────────────────────────────┤
│                  VERİ ERİŞİM KATMANI                   │
│   Entity Framework Core 8.0 (Code-First)               │
│   SQL Server (LocalDB / Express)                       │
└────────────────────────────────────────────────────────┘
```

---

## 🚀 Kurulum ve Çalıştırma

### 1. Projeyi Klonlayın
```bash
git clone https://github.com/enesaltndll/GoldBranchAI.git
cd GoldBranchAI
```

### 2. Veritabanı ve Paketleri Geri Yükleyin
```bash
dotnet restore
```

### 3. Uygulamayı Başlatın
```bash
dotnet run
```
*Uygulama varsayılan olarak `http://localhost:5000` (veya console ekranında belirtilen) portundan yayına başlayacaktır.*

---

## 🔑 Test Giriş Bilgileri

Sistemi değerlendirmek ve tüm modülleri test etmek için aşağıdaki varsayılan hesapları kullanabilirsiniz:

| Rol Yetkisi | E-posta Adresi | Şifre | Açıklama |
| :--- | :--- | :--- | :--- |
| **👑 Admin (Yönetici)** | `admin@test.com` | `123` | Tüm sisteme erişim, log inceleme ve DB sıfırlama yetkisi. |
| **💻 Geliştirici** | `ediz@test.com` | `ediz123` | Görev takibi, Zen modu, oyunlaştırma ve sohbet özellikleri. |

> [!NOTE]
> DB'yi temizlemek ve test verileriyle sıfırdan doldurmak için Admin rolüyle giriş yaptıktan sonra tarayıcı üzerinden `/Seed/ResetDatabase` adresini ziyaret edebilirsiniz.
> Güvenlik gereği yerel geliştirici kimlik anahtarları (API keyleri, SMTP şifreleri) `appsettings.Development.json` dosyasına alınmış ve bu dosya GitHub'dan hariç tutulmuştur.

---

## 👨‍💻 Geliştirici ve Akademik Kadro

* **Hazırlayan:** Enes ALTINDAL (Öğrenci No: 247017024)
* **Ders Sorumlusu:** Öğr. Gör. Ekrem SAYDAM
* **Kurum:** Sinop Üniversitesi / Ayancık Meslek Yüksekokulu / İnternet ve Ağ Teknolojileri Bölümü

---

## ❤️ Mezuniyet Raporu ve Teşekkür

Sinop Üniversitesi Ayancık Meslek Yüksekokulu çatısı altında geçirdiğimiz ve iki yılı aşkın bir süre boyunca her gününe ayrı bir emek sığdırdığımız eğitim hayatımızın sonuna gelmiş bulunuyoruz. Mezuniyetimizin nişanesi olan bu projenin hayata geçirilmesinde ve iki yıllık bu güzel süreçte:

* Bizlere sadece teknik bilgi ve teoriyi değil; mesleki vizyonu, disiplini ve mühendislik ahlakını aşılayan, her tıkandığımızda tecrübesiyle yolumuzu aydınlatan saygıdeğer hocamız **Öğr. Gör. Ekrem SAYDAM**'a,
* Sınıflarda omuz omuza verdiğimiz, birlikte sabahlayıp projeler ürettiğimiz, acı tatlı anıları paylaştığımız tüm değerli **öğrenci arkadaşlarımıza ve dönem dostlarımıza**,

Sonsuz teşekkür ve şükranlarımızı sunarız. Sinop Üniversitesinden mezun olurken, burada edindiğimiz dostlukları ve hocalarımızın paha biçilemez rehberliğini hayatımız boyunca gururla taşıyacağız.

🎓 *Geleceğin teknolojilerini inşa edeceğimiz yeni yollarda, hepimizin başarılara imza atması ve yollarımızın tekrar kesişmesi dileğiyle!*

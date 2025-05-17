# Emlak Yönetim Sistemi (C++)

Bu proje, C# dilinde geliştirilen bir **emlak takip ve yönetim otomasyon sisteminin** C++ diline çevrilmiş halidir. Konsol tabanlı bu uygulama, kullanıcıların satılık ve kiralık daire ilanlarını ekleyebileceği, arayabileceği ve güncelleyebileceği basit bir emlak otomasyonudur.

## Özellikler

- Kullanıcı girişi (Admin/Müşteri rolleri)
- Kiralık ve satılık ilan ekleme
- İl / ilçe / fiyat gibi kriterlere göre ilan arama
- İlanları listeleme
- İlan güncelleme ve silme
- Satış/kiralama durumu takibi
- Dosya tabanlı veri kaydı (satilik.txt / kiralik.txt)

## Kullanılan Teknolojiler

- **C++**
- Konsol uygulaması (CLI)
- Dosya işlemleri (`fstream`)
- Temel string ve vektör işlemleri (`<string>`, `<vector>`)

## Derleme ve Çalıştırma

```bash
g++ main.cpp -o EmlakApp
./EmlakApp

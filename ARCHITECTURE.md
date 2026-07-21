# TodoApp mimari rehberi

Bu çözüm sade bir **Clean Architecture + CQRS** yapısı kullanır. Bağımlılık yönü her zaman dış katmandan iç katmana doğrudur:

`API -> Application <- Persistence`

`Domain` hiçbir projeye bağımlı değildir.

## Klasörler

```text
TodoApp.Domain/
  Entities/                 İş verisi ve kuralları
  Enums/                    Domain sabitleri

TodoApp.Application/
  Abstractions/Persistence/ Uygulamanın ihtiyaç duyduğu veri erişim sözleşmeleri
  Common/                   Tüm feature'larda ortak davranışlar ve hatalar
  Features/Todos/           Todo iş senaryoları
    Commands/<Senaryo>/     Veri değiştiren istek, doğrulama ve handler
    Queries/<Senaryo>/      Sadece veri okuyan istek ve handler
  DTOs/                     API giriş/çıkış modelleri
  DependencyInjection.cs    Application katmanının tek DI giriş noktası

TodoApp.Persistence/
  Repositories/             Application sözleşmelerinin EF Core implementasyonları
  AppDbContext.cs           EF Core bağlantı noktası

TodoApp.API/
  Controllers/              HTTP isteğini MediatR request'ine çevirir
  Middleware/               HTTP seviyesindeki ortak davranışlar
  Program.cs                Uygulama bileşimi ve DI kayıtları
```

## Yeni bir Todo senaryosu ekleme

1. Veri değiştiriyorsa `Features/Todos/Commands/<Senaryo>` altında command, validator ve handler ekle.
2. Yalnızca veri okuyorsa `Features/Todos/Queries/<Senaryo>` altında query ve handler ekle.
3. Handler yalnızca Application soyutlamalarına bağımlı olsun; `DbContext`, HTTP sınıfları veya controller kullanma.
4. Yeni veri erişimi gerekiyorsa önce `Abstractions/Persistence/ITodoRepository` sözleşmesini güncelle, sonra Persistence implementasyonunu ekle.
5. Controller'da sadece `sender.Send(...)` çağrısı yap; iş kuralı ekleme.

## SOLID kuralları

- **S:** Controller HTTP'yi, handler tek senaryoyu, repository veri erişimini yönetir.
- **O:** Yeni senaryo mevcut handler'ları değiştirmek yerine yeni command/query ile eklenir.
- **L:** `ITodoRepository` implementasyonları aynı sözleşmenin yerine geçebilir.
- **I:** Application yalnızca ihtiyacı olan repository metotlarını ister.
- **D:** Application, `TodoRepository` yerine `ITodoRepository` soyutlamasına bağımlıdır.

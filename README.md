# Interaction System - Tolga

> Ludu Arts Unity Developer Intern Case

## Proje Bilgileri

| Bilgi | Deger |
|-------|-------|
| Unity Versiyonu | 6000.0.68f1 (Unity 6) |
| Render Pipeline | URP (Universal Render Pipeline) |
| Input System | New Input System (com.unity.inputsystem 1.18.0) |

---

## Kurulum

1. Repository'yi klonlayin:
```bash
git clone https://github.com/[username]/LuduArtsCase.git
```

2. Unity Hub'da projeyi acin (Unity 6000.0.68f1)
3. `Assets/InteractionSystem/Scenes/TestScene.unity` sahnesini acin
4. Play tusuna basin

---

## Nasil Test Edilir

### Kontroller

| Tus | Aksiyon |
|-----|---------|
| WASD | Hareket |
| Mouse | Bakis yonu |
| E | Etkilesim (Instant / Toggle) |
| E (basili tut) | Hold etkilesim (Chest) |
| Tab | Envanter ac/kapa |

### Test Senaryolari

1. **Door Test:**
   - Kapiya yaklasin, "Press E to Open" mesajini gorun
   - E'ye basin, kapi menteseden acilsin
   - Tekrar basin, kapi kapansin

2. **Key + Locked Door Test:**
   - Kilitli kapiya yaklasin, kirmizi "Locked - Red Key Required" mesajini gorun
   - Anahtari bulun ve E ile toplayin
   - Tab ile envanterde gorundugundan emin olun (ikon + isim)
   - Kilitli kapiya geri donun, simdi acilabilir

3. **Switch -> Door Test:**
   - Switch'e yaklasin ve E ile aktive edin
   - Bagli kapinin otomatik acildigini gorun
   - Tekrar E ile deaktive edin, kapi kapansin

4. **Chest Test:**
   - Sandiga yaklasin, "Hold E to Open (2s)" mesajini gorun
   - E'ye basili tutun, slider dolsun
   - Sandik acilsin ve icindeki item envantere eklensin
   - Tekrar deneyince "Already Opened" mesaji gorun

5. **Out of Range Test:**
   - Uzaktan bir nesneye bakin, "Too Far" mesajini gorun
   - Yaklastikca normal prompt'a gectigini gorun

---

## Mimari Kararlar

### Interaction System Yapisi

```
InteractionDetector (Player - SphereCast)
    |-- IInteractable tespit (menzil ici + menzil disi)
    |-- Input handling (Instant/Hold/Toggle)
    |-- Event-driven UI bildirimi
    |
IInteractable (Interface - explicit impl.)
    |-- Door     : Toggle, locked/unlocked, key kontrolu
    |-- KeyPickup: Instant, envantere ekleme
    |-- Switch   : Toggle, UnityEvent baglanti
    |-- Chest    : Hold, tek kullanimlik

PlayerInventory (ScriptableObject tabanli)
    |-- ItemData / KeyItemData
    |-- HasKeyOfType(), UseKey()

Bonus Sistemler:
    |-- InteractionHighlight  : Emission pulse efekti
    |-- InteractionSoundPlayer: ScriptableObject ses profilleri
```

**Neden bu yapiyi sectim:**
- Interface-based tasarim: Her interactable bagimsiz, SOLID uyumlu
- SphereCast: Raycast'tan daha toleransli, FPS icin ideal
- Explicit interface impl.: Ludu Arts standardi, encapsulation
- ScriptableObject item/sound: Data-driven, Inspector-friendly
- Event-driven UI: Loose coupling, performansli

**Trade-off'lar:**
- SphereCast her frame calisir ama tek ray, dusuk maliyet
- MaterialPropertyBlock ile highlight: GC allocation yok, performansli

### Kullanilan Design Patterns

| Pattern | Kullanim Yeri | Neden |
|---------|---------------|-------|
| Observer | Event system (OnTargetChanged, OnItemAdded vb.) | Loose coupling |
| Strategy | InteractionType (Instant/Hold/Toggle) | Farkli davranislar |
| Component | Highlight, SoundPlayer | Modularite |

---

## Ludu Arts Standartlarina Uyum

### C# Coding Conventions

| Kural | Uygulandi | Notlar |
|-------|-----------|--------|
| m_ prefix (private fields) | [x] | Tum private field'lar |
| s_ prefix (private static) | [x] | Bu projede kullanilmiyor |
| k_ prefix (private const) | [x] | k_SphereCastRadius, k_RotationSpeed vb. |
| Region kullanimi | [x] | Fields > Events > Properties > Unity Methods > Methods > Interface Impl |
| Region sirasi dogru | [x] | Standart siralama |
| XML documentation | [x] | Tum public API'ler |
| Silent bypass yok | [x] | Debug.LogError/LogWarning ile loglama |
| Explicit interface impl. | [x] | IInteractable tum nesnelerde |

### Naming Convention

| Kural | Uygulandi | Ornekler |
|-------|-----------|----------|
| P_ prefix (Prefab) | [x] | P_Door, P_Chest, P_Switch, P_Key |
| M_ prefix (Material) | [x] | M_Door, M_Key_Red, M_Key_Blue |
| SO isimlendirme | [x] | ItemData, KeyItemData, InteractionSoundData |

### Prefab Kurallari

| Kural | Uygulandi | Notlar |
|-------|-----------|--------|
| Transform (0,0,0) | [x] | Tum prefab'lar |
| Pivot dogru | [x] | Kapi mentesede, sandik kapak arkada |
| Collider tercihi | [x] | Box Collider kullanildi |
| Hierarchy yapisi | [x] | Root > Pivot > Visual |


### Zorlandığım Noktalar
> [Çok komplike bir sistem yapmadığım için zorlandığım bir alan olmadı]

## Tamamlanan Ozellikler

### Zorunlu (Must Have)

- [x] Core Interaction System
  - [x] IInteractable interface
  - [x] InteractionDetector (SphereCast)
  - [x] Range kontrolu + Out of Range feedback

- [x] Interaction Types
  - [x] Instant
  - [x] Hold
  - [x] Toggle

- [x] Interactable Objects
  - [x] Door (locked/unlocked, key ile acma)
  - [x] Key Pickup (2 tip: Red, Blue)
  - [x] Switch/Lever (UnityEvent ile kapi baglantisi)
  - [x] Chest/Container (2sn hold, tek kullanimlik)

- [x] UI Feedback
  - [x] Interaction prompt (dinamik text)
  - [x] Hold progress bar (Slider)
  - [x] Out of Range feedback
  - [x] Cannot interact feedback (kirmizi text)

- [x] Simple Inventory
  - [x] Key toplama ve saklama
  - [x] UI listesi (ikon + isim)
  - [x] ScriptableObject item tanimlari

### Bonus (Nice to Have)

- [x] Animation entegrasyonu  - Kapi rotasyon, sandik kapak, lever hareket
- [x] Sound effects integration  - ScriptableObject tabanli ses profilleri
- [x] Multiple keys / color-coded  - Red, Blue, Gold key turleri
- [x] Interaction highlight  - Emission pulse efekti
- [x] Chained interactions  - Switch -> Door UnityEvent baglantisi

---
## Bilinen Limitasyonlar

### Tamamlanamayan Özellikler
1. [Seve sistemi] - [Normalde playerpref e ya da json dosyası olarak verileri saklayabilrdim ama sistemde buna ihtiyaç duyan bir yapı olmadığı için yapmadım]
2. [Çevre tasarımı] - [Case çalışmasının bununla alakalı bir isteğini görmedim]



### Iyilestirme Onerileri
- Envanter sistemi stack/miktar destegi eklenebilir
- Object pooling ile pickup nesneleri optimize edilebilir
- Highlight icin outline shader daha iyi gorsel sonuc verir
- Fps kamerası daha da geliştirile bilir ve yürürekn koşarken daha iyi hissedilmesi sağlana bilir

## Ekstra Özellikler

Zorunlu gereksinimlerin dışında eklediklerim:

1. Open Only By Switch (Sadece şalter ile açılan kapı)
Açıklama: Door component’inde "Open Only By Switch" ile kapı sadece Switch/Lever’ın SetOpen(bool) çağrısı ile açılır. Oyuncu E tuşu veya anahtar kullanamaz. Görünen mesaj: "Use Switch to Open".
Neden ekledim: Elektrikli kapı, uzaktan kumandalı kapı gibi farklı kapı tipleri için esneklik sağlamak. Case’de switch–door bağlantısı vardı, bu da ek bir kapı modu olarak eklendi.

2. Inventory Item Icons
Açıklama: Envanter listesinde her item’ın ScriptableObject’teki Icon sprite’ı gösterilir. InventorySlotUI bileşeni Image + TextMeshPro ile ikon ve isim gösterir.
Neden ekledim: Basit liste yerine görsel geri bildirim ve daha iyi kullanıcı deneyimi sağlamak. ItemData / KeyItemData üzerindeki mevcut Icon alanı kullanılıyor.

## Dosya Yapisi

```
Assets/
├── InteractionSystem/
│   ├── Scripts/
│   │   ├── Runtime/
│   │   │   ├── Core/
│   │   │   │   ├── IInteractable.cs
│   │   │   │   ├── InteractionType.cs
│   │   │   │   ├── KeyType.cs
│   │   │   │   ├── ItemData.cs
│   │   │   │   ├── KeyItemData.cs
│   │   │   │   ├── InteractionHighlight.cs
│   │   │   │   ├── InteractionSoundData.cs
│   │   │   │   └── InteractionSoundPlayer.cs
│   │   │   ├── Interactables/
│   │   │   │   ├── Door.cs
│   │   │   │   ├── KeyPickup.cs
│   │   │   │   ├── Switch.cs
│   │   │   │   └── Chest.cs
│   │   │   ├── Player/
│   │   │   │   ├── SimplePlayerController.cs
│   │   │   │   ├── InteractionDetector.cs
│   │   │   │   └── PlayerInventory.cs
│   │   │   └── UI/
│   │   │       ├── InteractionPromptUI.cs
│   │   │       ├── HoldProgressBarUI.cs
│   │   │       ├── InventoryUI.cs
│   │   │       └── InventorySlotUI.cs
│   │   └── Editor/
│   ├── ScriptableObjects/Items/
│   ├── Prefabs/
│   │   ├── Interactables/
│   │   ├── UI/
│   │   └── Player/
│   ├── Materials/
│   └── Scenes/
│       └── TestScene.unity
Docs/
├── CSharp_Coding_Conventions.md
├── Naming_Convention_Kilavuzu.md
└── Prefab_Asset_Kurallari.md
README.md
PROMPTS.md
.gitignore
```

---
## İletişim

| Bilgi | Değer |
|-------|-------|
| Ad Soyad | [Tolga Yıldız] |
| E-posta | [email@
example.com] |
| LinkedIn | [profil linki] |
| GitHub | [github.com/username] |


---

*Bu proje Ludu Arts Unity Developer Intern Case icin hazirlanmistir.*

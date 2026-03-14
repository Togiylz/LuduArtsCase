# LLM Kullanim Dokumantasyonu

## Ozet

| Bilgi | Deger |
|-------|-------|
| Toplam prompt sayisi | 10+ |
| Kullanilan araclar | Cursor (Claude) |
| En cok yardim alinan konular | Mimari planlama, kod yazimi, bug fix, bonus ozellikler |
| Tahmini LLM ile kazanilan sure | 4-5 saat |

---

## Prompt 1: Proje Setup ve Mimari Planlama

**Arac:** Cursor (Claude)
**Tarih/Saat:** 2026-03-14 15:30

**Prompt:**
```
Case calismasina basliyorum. Adim adim yapilacaklari yapiyoruz 1. adimdan baslayalim.
(Referans: CASE_Unity_Interaction_System.md, CSharp_Coding_Conventions.md,
Naming_Convention_Kilavuzu.md, Prefab_Asset_Kurallari.md)
```

**Alinan Cevap (Ozet):**
```
Proje klasor yapisi olusturuldu, mimari plan belirlendi:
- Interface-based IInteractable, SphereCast detection, ScriptableObject item sistemi
- README.md ve PROMPTS.md sablonlari olusturuldu
```

**Nasil Kullandim:**
- [ ] Direkt kullandim
- [x] Adapte ettim
- [ ] Reddettim

**Aciklama:**
> Klasor yapisi case dokumanindaki spesifikasyona uygun olarak olusturuldu.
> Mimari kararlari (raycast vs trigger, interface vs abstract class) tartisarak karar verdim.

---

## Prompt 2: Core Interaction System Kodlari

**Arac:** Cursor (Claude)
**Tarih/Saat:** 2026-03-14 15:45

**Prompt:**
```
2. adima gec - Core system kodlarini yaz
(IInteractable, InteractionType, InteractionDetector, PlayerInventory, ItemData, KeyItemData)
```

**Alinan Cevap (Ozet):**
```
8 core dosya olusturuldu: InteractionType enum, IInteractable interface,
InteractionDetector (SphereCast), PlayerInventory, ItemData/KeyItemData SO,
SimplePlayerController, KeyType enum
```

**Nasil Kullandim:**
- [ ] Direkt kullandim
- [x] Adapte ettim
- [ ] Reddettim

**Aciklama:**
> Kodlar Ludu Arts convention'larina uygun yazildi (m_, k_ prefix, region, XML doc).
> InteractionDetector'da SphereCast tercih ettim cunku FPS bakis yonunde daha toleransli.

---

## Prompt 3: Interactable Nesneler

**Arac:** Cursor (Claude)
**Tarih/Saat:** 2026-03-14 16:00

**Prompt:**
```
3. adima gec - Door, KeyPickup, Switch, Chest kodlarini yaz
```

**Alinan Cevap (Ozet):**
```
4 interactable implement edildi:
- Door: Toggle, locked/unlocked, key kontrolu, rotasyon animasyonu
- KeyPickup: Instant, envantere ekleme, havada donme efekti
- Switch: Toggle, UnityEvent ile Inspector'dan baglanti
- Chest: Hold (2sn), tek kullanimlik, kapak animasyonu
```

**Nasil Kullandim:**
- [ ] Direkt kullandim
- [x] Adapte ettim
- [ ] Reddettim

**Aciklama:**
> Door ve Chest icin pivot-based rotasyon sistemi kullanildi.
> Switch'te UnityEvent secimi Inspector'dan baglanti kurmak icin yapildi.

---

## Prompt 4: UI Feedback Sistemi

**Arac:** Cursor (Claude)
**Tarih/Saat:** 2026-03-14 16:15

**Prompt:**
```
4. adima gec - UI feedback sistemi (prompt, progress bar, inventory listesi)
```

**Alinan Cevap (Ozet):**
```
3 UI bileseni: InteractionPromptUI (prompt + cannot interact),
HoldProgressBarUI (fill image), InventoryUI (Tab ile acilir liste)
```

**Nasil Kullandim:**
- [ ] Direkt kullandim
- [x] Adapte ettim
- [ ] Reddettim

**Aciklama:**
> HoldProgressBarUI'yi sonradan Slider tabanli olarak degistirdim
> cunku Image fillAmount setup'i daha karmasikti.

---

## Prompt 5: Input System Gecisi

**Arac:** Cursor (Claude)
**Tarih/Saat:** 2026-03-14 16:30

**Prompt:**
```
InvalidOperationException: You are trying to read Input using the UnityEngine.Input class,
but you have switched active Input handling to Input System package in Player Settings.
Yeni input sistemini kullaniyorum degil mi
```

**Alinan Cevap (Ozet):**
```
3 dosya yeni Input System'e gecti:
- Input.GetAxis -> Mouse.current.delta / Keyboard.current
- Input.GetKeyDown -> Keyboard.current[Key].wasPressedThisFrame
- KeyCode -> Key enum
```

**Nasil Kullandim:**
- [x] Direkt kullandim
- [ ] Adapte ettim
- [ ] Reddettim

**Aciklama:**
> Proje yeni Input System kullaniyordu, eski API calismiyor.
> Degisiklik dogrudandi, direkt uyguladim.

---

## Prompt 6: CannotInteract ve ProgressBar Bug Fix

**Arac:** Cursor (Claude)
**Tarih/Saat:** 2026-03-14 16:45

**Prompt:**
```
CannotInteractText bazen calismiyor, progress bar duzgun calismiyor
```

**Alinan Cevap (Ozet):**
```
2 bug tespit edildi:
1. Door.CanInteract her zaman true donuyordu - kilitli kapilar icin duzeltildi
2. InteractionPromptUI.UpdatePromptText timer'i her frame sifirliyordu - yeniden yazildi
```

**Nasil Kullandim:**
- [x] Direkt kullandim
- [ ] Adapte ettim
- [ ] Reddettim

**Aciklama:**
> Bug analizleri dogru ve fix'ler calisti.

---

## Prompt 7: Pivot/Mentese Duzeltmesi

**Arac:** Cursor (Claude)
**Tarih/Saat:** 2026-03-14 17:00

**Prompt:**
```
Kapilar ve sandik aciliyor ama gercek kapi gibi mentese noktasindan acilmiyor
```

**Alinan Cevap (Ozet):**
```
Prefab hierarchy duzeltmesi onerildi:
- DoorPivot (Empty, mentese kenarinda) > Visual (offset)
- LidPivot (Empty, arka kenarda) > Lid (offset)
```

**Nasil Kullandim:**
- [x] Direkt kullandim
- [ ] Adapte ettim
- [ ] Reddettim

**Aciklama:**
> Unity Editor'da hierarchy'yi duzeltip pivot objelerini olusturdugumda
> kapi ve sandik dogru noktadan donmeye basladi.

---

## Prompt 8: Out of Range + Inventory Ikonlari

**Arac:** Cursor (Claude)
**Tarih/Saat:** 2026-03-14 17:30

**Prompt:**
```
Out of Range feedback ekle, inventory'de itemlerin fotolari da gozuksun
```

**Alinan Cevap (Ozet):**
```
- InteractionDetector'a 2x menzil detection + OnOutOfRangeTargetChanged event'i eklendi
- InventorySlotUI component'i olusturuldu (Image + TextMeshPro)
- InventoryUI artik ikon + isim gosteriyor
```

**Nasil Kullandim:**
- [x] Direkt kullandim
- [ ] Adapte ettim
- [ ] Reddettim

**Aciklama:**
> Case'de zorunlu olan Out of Range feedback eksikti, eklendi.
> Inventory ikonlari ItemData.Icon'dan cekildi.

---

## Prompt 9: Bonus Ozellikler (Highlight, Sound, Save/Load)

**Arac:** Cursor (Claude)
**Tarih/Saat:** 2026-03-14 17:45

**Prompt:**
```
Hepsini ekle, sound sistemini profesyonel bir game developer gibi yaz
```

**Alinan Cevap (Ozet):**
```
3 bonus sistem eklendi:
- InteractionHighlight: Emission pulse efekti, MaterialPropertyBlock
- InteractionSoundData/Player: ScriptableObject ses profilleri, AudioSource pooling,
  pitch variation, ayri loop source
- InteractionSaveSystem: JSON + PlayerPrefs, SerializableDictionary
```

**Nasil Kullandim:**
- [ ] Direkt kullandim
- [x] Adapte ettim
- [ ] Reddettim

**Aciklama:**
> Sound sistemi profesyonel: ScriptableObject tabanli ses profili,
> rastgele clip secimi, pitch variation, ayri loop AudioSource.
> Highlight MaterialPropertyBlock ile GC-free.

---

## Genel Degerlendirme

### LLM'in En Cok Yardimci Oldugu Alanlar
1. Proje yapisinin hizlica olusturulmasi ve convention uyumu
2. Bug analizi ve root cause tespiti
3. Profesyonel ses sistemi mimarisi (ScriptableObject tabanli)

### LLM'in Yetersiz Kaldigi Alanlar
1. Unity Editor ici isler (prefab kurma, sahne duzeni) - bunlari manuel yaptim
2. Ilk seferde dogru input system kullanmadi (eski API ile yazdi)

### LLM Kullanimi Hakkinda Dusuncelerim
> LLM'i en etkili adim adim, spesifik sorularla kullandim.
> "Bana interaction system yaz" yerine her adimi ayri ayri isteyerek
> kodu anladigimdan emin oldum. Bug fix'lerde LLM'in analiz yeteneginden
> cok fayda gordum. Ses sistemi gibi daha az tecrubeli oldugum alanlarda
> profesyonel mimari ogrendim. LLM'siz tahminen 8-10 saat surerdi.

---

*Bu sablon Ludu Arts Unity Intern Case icin hazirlanmistir.*

# LLM Kullanim Dokumantasyonu

## Ozet

| Bilgi | Deger |
|-------|-------|
| Toplam prompt sayisi | 1 |
| Kullanilan araclar | Cursor (Claude) |
| En cok yardim alinan konular | Proje setup, mimari planlama |
| Tahmini LLM ile kazanilan sure | 0.5 saat |

---

## Prompt 1: Proje Setup ve Mimari Planlama

**Arac:** Cursor (Claude)
**Tarih/Saat:** 2026-03-14 15:30

**Prompt:**
```
Case calismasina basliyorum. Adim adim yapilacaklari yapiyoruz 1. adimdan baslayalim.
(Referans dosyalar: CASE_Unity_Interaction_System.md, CSharp_Coding_Conventions.md,
Naming_Convention_Kilavuzu.md, Prefab_Asset_Kurallari.md, PROMPTS_TEMPLATE.md, README_TEMPLATE.md)
```

**Alinan Cevap (Ozet):**
```
- Case dokumanlarinin analizi yapildi
- Proje klasor yapisi olusturuldu (Scripts/Runtime/Core, Interactables, Player, UI vb.)
- Mimari plan belirlendi: Interface-based IInteractable, Raycast-based detection,
  ScriptableObject item sistemi
- README.md ve PROMPTS.md sablonlari proje bilgileriyle guncellendi
```

**Nasil Kullandim:**
- [ ] Direkt kullandim
- [x] Adapte ettim
- [ ] Reddettim

**Aciklama:**
> Ilk adimda LLM'den proje yapisini olusturmasini ve mimari plani belirlemesini istedim.
> Klasor yapisi case dokumanindaki spesifikasyona uygun olarak olusturuldu.
> Mimari kararlari (raycast vs trigger, interface vs abstract class) tartisarak karar verdim.

**Yapilan Degisiklikler:**
> Klasor yapisi case dokumanindaki ornek yapiyla birebir uyumlu hale getirildi.
> README.md icerigi Ludu Arts standartlarina uygun sekilde duzenlendi.

---

## Genel Degerlendirme

### LLM'in En Cok Yardimci Oldugu Alanlar
1. Proje yapisinin hizlica olusturulmasi
2. Mimari karar surecinde alternatiflerin degerlendirilmesi
3. Coding convention'larin tutarli uygulanmasi

### LLM'in Yetersiz Kaldigi Alanlar
(Proje ilerledikce guncellenecek)

### LLM Kullanimi Hakkinda Dusuncelerim
> (Proje sonunda guncellenecek)

---

*Bu sablon Ludu Arts Unity Intern Case icin hazirlanmistir.*

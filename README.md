# Simulacija Pametnih Kuća

Ovaj projekat omogućava simulaciju pametnih kuća, upravljanje uređajima u različitim sobama, praćenje potrošnje energije i generisanje računa za energiju od strane različitih dobavljača.

## Ključne funkcionalnosti

- Upravljanje pametnim uređajima u kućama (uključivanje/isključivanje uređaja ili cijelih soba)
- Simulacija potrošnje energije i kreiranje računa za kuće
- Upravljanje dobavljačima energije (promjena osnovne cijene, porezne marže i formule obračuna)
- Prikaz svih kuća, njihovih soba i uređaja
- Pregled dostupnih dobavljača energije
- Prikaz računa za određene dobavljače
- Izvođenje upita, uključujući:
  - Koja kuća je najviše potrošila energije?
  - Koji dobavljač je ostvario najveći prihod?
  - Rangiranje kuća prema potrošnji energije u određenom periodu

## Struktura projekta

```
📂 src/
 ├── org/
 │   ├── House/            # Klase vezane za kuće i sobe
 │   ├── Devices/          # Pametni uređaji
 │   ├── Suppliers/        # Dobavljači energije i računi
 │   ├── Exceptions/       # Definicije izuzetaka
 │   ├── Main.java         # Ulazna tačka programa
 │   ├── Simulation.java   # Glavna simulacija
```

## Buduća poboljšanja

- Omogućavanje učitavanja podataka iz datoteka
- Optimizacija algoritama za proračun potrošnje
- Bolja vizualizacija podataka o potrošnji energije

## Autor
Ovaj projekat je razvijen za potrebe upravljanja pametnim kućama i analize potrošnje energije.
Alem Aljović
---
Za sva pitanja ili prijedloge, slobodno se javite! 😊


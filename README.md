SecretService API


Egy egszerű web api amely egy GET és egy POST metódust tartalmaz.
Működését tekintve a POST metódussal egy secret-et menthetünk el az adatbázisba. Mentés során meg kell adni magát a titkot ez úgynevezett hash-t amely azonosítja a titkot. Valamint meg kell adni magát a titkos szöveget, egy számot amely a megadja hányszor lehet megtekinteni, és egy további egész számot amely az úgynevezett lejárati időt határozza meg. A lejárati időnek megadott szám percben értendő és hozzá adódik az létrehozás idejéhez.
A GET metódussal az elmentett/létrehozott titkokat tekinthetjük meg ha még elérhetők. Egy titok lekéréséhez egy hash megadása szükséges a GET metódusban.

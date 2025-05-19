char* GenerisiID(int ID) {
    char str[10];
    if (ID < 10)
        sprintf_s(str, "ID#000-%d", ID);
    else if (ID < 100)
        sprintf_s(str, "ID#00-%d", ID);
    else if (ID < 1000)
        sprintf_s(str, "ID#0-%d", ID);
    else
        sprintf_s(str, "ID#-%d", ID);
    
    return GetNizKaraktera(str);
}

friend ostream& operator << (ostream& COUT, const Prventstvo& obj) {
    for (int i = 0; i < obj._utakmice.getTrenutno(); i++) {
        COUT << crt;
        COUT << obj._utakmice.getElement1(i) << " " << obj._utakmice.getElement1(i).GetBrojGolova() 
             << " : " << obj._utakmice.getElement2(i).GetBrojGolova() << " " 
             << obj._utakmice.getElement2(i) << crt;

        // Print scorers for team 1
        for (const auto& igrac : obj._utakmice.getElement1(i).GetIgraci()) {
            for (const auto& pogodak : igrac.GetPogoci()) {
                COUT << igrac.GetImePrezime() << endl;
            }
        }

        // Print scorers for team 2
        for (const auto& igrac : obj._utakmice.getElement2(i).GetIgraci()) {
            for (const auto& pogodak : igrac.GetPogoci()) {
                COUT << string(26, ' ') << igrac.GetImePrezime() << endl;
            }
        }
    }
    return COUT;
} 
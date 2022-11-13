//Kalman filter implementation in c
#include <iostream>
#include <conio.h>
using namespace std;

double kalman_old = 68;
double err_last = 2;
double R_HataMiktari = 3;
double Q_HataMiktari = 0.2;

double kalman_filter(double);

int main() {

    const int NUMBERS_SIZE = 6;
    double numbers[NUMBERS_SIZE] = { 75, 71, 70, 74, 73, 75 };

    double yeni[NUMBERS_SIZE];

    for (int i = 0; i < (NUMBERS_SIZE); i++) {
        yeni[i] = kalman_filter(numbers[i]);

        cout << endl << "ilk = " << numbers[i] << " son = " << yeni[i] << endl;

    }
    char a;
    cin >> a;
    return 0;
}

double kalman_filter(double input) {

    double err_estimate;
    double kalman_calc;
    double kalman_gain;

    err_estimate = err_last + Q_HataMiktari;      //yeni kovaryans degeri belirlenir. Q = 0.50 alinmistir, Q sistemden kaynakli hata miktari
    kalman_gain = err_estimate / (err_estimate + R_HataMiktari); //kalman kazanci hesaplanir. R=0.9 alinmistir, R olcumden kaynakli hata miktari
    kalman_calc = kalman_old + (kalman_gain * (input - kalman_old)); //kalman degeri hesaplanir
    err_estimate = (1 - kalman_gain) * err_estimate; //yeni kovaryans degeri hesaplanir

    err_last = err_estimate; //yeni degerler bir sonraki döngüde kullanilmak üzere kaydedilir 
    kalman_old = kalman_calc;

    return kalman_calc;
}
#ifndef SMU_H
#define SMU_H

#include <math.h>
#include <stdint.h>
#include <stdbool.h>
#include "DAC8760.h"
#include "ADS1262.h"
#include "DWIN.h"
#include "usb_device.h"

#define LSV_InitialVoltage_VP   0x2100
#define LSV_FinalVoltage_VP     0x2103
#define LSV_StepSize_VP         0x2106
#define LSV_ScanRate_VP         0x2109

#define CV_InitialVoltage_VP    0x2110
#define CV_FinalVoltage_VP      0x2119
#define CV_Peak1_VP             0x2113
#define CV_Peak2_VP             0x2116
#define CV_StepSize_VP          0x211C
#define CV_ScanRate_VP          0x2120
#define CV_Cycles_VP            0x2123

#define DPV_InitialVoltage_VP   0x2126
#define DPV_FinalVoltage_VP     0x2129
#define DPV_StepSize_VP         0x212C
#define DPV_PulseV_VP           0x2130
#define DPV_PulseTime_VP        0x2133
#define DPV_BaseTime_VP         0x2136

#define SWV_InitialVoltage_VP   0x2139
#define SWV_FinalVoltage_VP     0x213C
#define SWV_StepSize_VP         0x2140
#define SWV_AmplitudeV_VP       0x2143
#define SWV_Frequency_VP        0x2146

#define CP_ConstI_VP            0x2149
#define CP_SampleT_VP           0x214C
#define CP_SampleP_VP           0x2150

#define LSP_InitialI_VP         0x2153
#define LSP_FinalI_VP           0x2156
#define LSP_StepSize_VP         0x2159
#define LSP_ScanRate_VP         0x215C

#define CyP_InitialI_VP         0x2160
#define CyP_FinalI_VP           0x2169
#define CyP_Peak1_VP            0x2163
#define CyP_Peak2_VP            0x2166
#define CyP_StepSize_VP         0x216C
#define CyP_ScanRate_VP         0x2170
#define CyP_Cycles_VP           0x2173

typedef struct
{
    DAC8760_t Potentiostat_DAC; // DAC that will be used in the potentiostat
    DAC8760_t Galvanostat_DAC;  // DAC that will be used in the galvanostat
    uint16_t Ammeter_AIN;       // Analog IN used for the ammeter
    uint16_t Voltmeter_AINN;    // Negative analog IN used for the voltmeter
    uint16_t Voltmeter_AINP;    // Positive analog IN used for the voltmeter
    bool channel1;              // bool to identifie which channel it is, then its possible to use if to know which channel
} channel_t;

typedef struct
{
    float *values;
    size_t length;
    int timeStep;
    channel_t channel;
    bool potentiostat; //if it is a potentiostat measure, potentiostat=1, if it is a galvanostat measure, potentiostat=0
} ArrayMeasurementData;

#pragma region techniques structs
typedef struct
{
    channel_t channel;
    float initialVoltage;
    float finalVoltage;
    float voltageStep;
    float scanRate;
} LSV;

typedef struct
{
    channel_t channel;
    float initialVoltage;
    float peak1Voltage;
    float peak2Voltage;
    float finalVoltage;
    float voltageStep;
    float scanRate;
    float cycles;
} CV;

typedef struct
{
    channel_t channel;
    float initialVoltage;
    float finalVoltage;
    float voltageStep;
    float voltagePulse;
    float pulseTime;
    float baseTime;
} DPV;

typedef struct
{
    channel_t channel;
    float initialVoltage;
    float finalVoltage;
    float voltageStep;
    float voltageAmplitude;
    float frequency;
} SWV;

typedef struct
{
    channel_t channel;
    float constCurrent;
    float sampleTime;
    float samplePeriod;
} CP;

typedef struct
{
    channel_t channel;
    float initialCurrent;
    float finalCurrent;
    float currentStep;
    float scanRate;
} LSP;

typedef struct
{
    channel_t channel;
    float initialCurrent;
    float peak1Current;
    float peak2Current;
    float finalCurrent;
    float currentStep;
    float scanRate;
    float cycles;
} CyP;

#pragma endregion

extern channel_t Channel1;
extern channel_t Channel2;
extern DAC8760_t Ch1_Potentiostat_DAC; // Potentiostat Channel 1
extern DAC8760_t Ch1_Galvanostat_DAC;  // Galvanostat Channel 1
extern DAC8760_t Ch2_Potentiostat_DAC; // Potentiostat Channel 2
extern DAC8760_t Ch2_Galvanostat_DAC;  // Galvanostat Channel 2

extern char input[64];
extern uint8_t USB_newData;

extern TIM_HandleTypeDef htim2;
extern UART_HandleTypeDef huart1;
extern int currentAmmeterResistorIndex;
extern float currentAmmeterResistor;
extern float ammeterResistors [7]; //{10, 100, 1000, 10000, 100000, 1000000, 10000000};
extern float currentSourceResistor;
extern int pageID;
extern uint8_t dwinChannel;
extern bool HMIflag;   // HMI flag -> 0 PC ; 1 DWIN display

void SMU_Init(SPI_HandleTypeDef DAC8760hspi);
void Channel1_Init(channel_t *channel, DAC8760_t *potentiostat_DAC, DAC8760_t *galvanostat_DAC, uint16_t ammeter_AIN,
                  uint16_t voltmeter_AINP, uint16_t voltmeter_AINN, SPI_HandleTypeDef DAC8760hspi);
void Channel2_Init(channel_t *channel, DAC8760_t *potentiostat_DAC, DAC8760_t *galvanostat_DAC, uint16_t ammeter_AIN,
                  uint16_t voltmeter_AINP, uint16_t voltmeter_AINN, SPI_HandleTypeDef DAC8760hspi);

double SMU_VoltageRead(channel_t channel);
double SMU_CurrentRead(channel_t channel);
void SMU_setAmmeterResistor(int currentAmmeterResistorIndex, channel_t channel);

void SMU_SetVoltage(float output, channel_t channel);
void SMU_SetCurrent(float output, channel_t channel);

void SMU_LSV(LSV lsv); // Linear Sweep Voltammetry
void SMU_CV(CV cv);  // Cylic Voltammetry
void SMU_DPV(DPV dpv); // Differential Pulse Voltammetry
void SMU_SWV(SWV swv); // Square Wave Voltammetry

void SMU_CP(CP cp);  // Chronopotentiometry
void SMU_LSP(LSP lsp); // Linear Sweep Potentiometry
void SMU_CyP(CyP cyp);  // Cyclic Potentiometry

void SMU_dualChannelMeasure(ArrayMeasurementData channel1Data, ArrayMeasurementData channel2Data);

// set parameters from DWIN
void waitForDwin();
void waitForDwinTechnique();
void LSV_DWIN_Page();
void getLSVParameters_DWIN();
void getCVParameters_DWIN();
void getDPVParameters_DWIN();
void getSWVParameters_DWIN();

// set parameters from PC
void setLSVParameters(LSV *lsv, char *parameters, bool channel);
void setCVParameters(CV *cv, char *parameters, bool channel);
void setDPVParameters(DPV *dpv, char *parameters, bool channel);
void setSWVParameters(SWV *swv, char *parameters, bool channel);
void setCPParameters(CP *cp, char *parameters, bool channel);
void setLSPParameters(LSP *lsp, char *parameters, bool channel);
void setCyPParameters(CyP *cyp, char *parameters, bool channel);

ArrayMeasurementData BuildLSVarray(LSV lsv);
ArrayMeasurementData BuildCVarray(CV cv);
ArrayMeasurementData BuildCParray(CP cp);
ArrayMeasurementData BuildLSParray(LSP lsp);
ArrayMeasurementData BuildCyParray(CyP cyp);

void SMU_ProcessComandIT();
void SMU_AbortIT(); // Interruption to abort the measurement

void dwin_interface();
void pc_interface();

void SMU_UpdateInterfacePC();
void SMU_UpdateInterfaceDisplay();

#endif

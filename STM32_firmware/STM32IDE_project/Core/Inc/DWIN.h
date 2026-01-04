#ifndef DWIN_H
#define DWIN_H

#include "stm32f4xx_hal.h"
#include <stdint.h>
#include "usb_device.h"
#include <string.h>

extern UART_HandleTypeDef huart1;

void readVP(uint16_t vp, uint8_t numBytes);
void setDisplayPage(uint8_t pageId);
void readDisplayFloat(float *myFloat);
char* getVP();
void dwinSendDouble(uint16_t vp, double doubleNumber);
void doubleToHex(double f, uint8_t* hex);

#endif

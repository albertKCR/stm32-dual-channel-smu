#include "DAC8760.h"

void DAC8760_Init(DAC8760_t *dac, uint16_t latch, GPIO_TypeDef *port, SPI_HandleTypeDef hspi)
{
    dac->latch_pin = latch;
    dac->latch_port = port;
    dac->hspi = hspi;

    DAC8760_LATCH_LOW(dac);

    DAC8760_WriteRegister(dac, ADDR_RESET, 0x0001); // Software reset
    DAC8760_WriteRegister(dac, ADDR_CTRL, 0x1003); // 0001 0000 0000 0011 => 0V when CLR and RANGE bits 011 (-10V to +10V)
    DAC8760_WriteRegister(dac, ADDR_DATA, 0x0000); // set the output voltage

    DAC8760_LATCH_HIGH(dac);
    DAC8760_LATCH_LOW(dac);
}

void DAC8760_WriteVoltage(DAC8760_t *dac, uint16_t value)
{
    DAC8760_WriteRegister(dac, ADDR_DATA, value);
}

void DAC8760_WriteRegister(DAC8760_t *dac, uint8_t reg, uint16_t value)
{
    uint8_t data[3];
    data[0] = reg;
    data[1] = value >> 8;
    data[2] = value & 0x00FF;

    HAL_SPI_Transmit(&dac->hspi, data, 3, 10);

    DAC8760_LATCH_HIGH(dac);
    DAC8760_LATCH_LOW(dac);
}

uint16_t DAC8760_ReadRegister(DAC8760_t *dac, uint8_t reg)
{
    uint8_t value[2];
    uint8_t data[3];

    data[0] = 0x02;
    data[1] = 0x00;
    data[2] = reg & 0x1F;

    HAL_SPI_Transmit(&dac->hspi, data, 3, 10);

    DAC8760_LATCH_HIGH(dac);
    DAC8760_LATCH_LOW(dac);

    HAL_SPI_Receive(&dac->hspi, value, 2, 10);

    return (value[0] << 8) | value[1];
}

void DAC8760_LATCH_LOW(DAC8760_t *dac)
{
    HAL_GPIO_WritePin(dac->latch_port, dac->latch_pin, GPIO_PIN_RESET);
}

void DAC8760_LATCH_HIGH(DAC8760_t *dac)
{
    HAL_GPIO_WritePin(dac->latch_port, dac->latch_pin, GPIO_PIN_SET);
}

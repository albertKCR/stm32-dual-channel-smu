#include "ADS1262.h"

uint8_t spi_rx_buffer[3];             // Buffer for SPI received data
uint8_t spi_tx_buffer[3];             // Buffer for SPI transmit data
volatile uint8_t data_ready_flag = 0; // Flag set when new data is available
char charData[15];

// Functions to select/deselect ADS1262
void ADS1262_Select()
{
    HAL_GPIO_WritePin(ADS1262_CS_GPIO_Port, ADS1262_CS_Pin, GPIO_PIN_RESET);
}

void ADS1262_Deselect()
{
    HAL_GPIO_WritePin(ADS1262_CS_GPIO_Port, ADS1262_CS_Pin, GPIO_PIN_SET);
}

// Set START pin
void ADS1262_START()
{
    HAL_GPIO_WritePin(ADS1262_START_GPIO_Port, ADS1262_START_Pin, GPIO_PIN_SET);
}

// Reset START pin
void ADS1262_HARD_STOP()
{
    HAL_GPIO_WritePin(ADS1262_START_GPIO_Port, ADS1262_START_Pin, GPIO_PIN_RESET);
}

// SPI Transmit & Receive Complete Callback
void HAL_SPI_TxRxCpltCallback(SPI_HandleTypeDef *hspi)
{
    if (hspi->Instance == SPI1)
    {
        data_ready_flag = 1; // Set flag to indicate data is received
        ADS1262_Deselect();
    }
}

// DRDY Interrupt Callback
void HAL_GPIO_EXTI_Callback(uint16_t GPIO_Pin)
{
    if (GPIO_Pin == ADS1262_DRDY_Pin)
    {
        data_ready_flag = 1;
    }
}

// Function to Write a Register
void ADS1262_WriteRegister(uint8_t reg, uint8_t value)
{
    uint8_t cmd[3];
    cmd[0] = 0x40 | reg;
    cmd[1] = 0x00;
    cmd[2] = value;
    ADS1262_Select();
    HAL_SPI_Transmit(&hspi1, cmd, 3, 10); // Envia comando e valor

    ADS1262_Deselect();
}

// Function to Read a Register
uint8_t ADS1262_ReadRegister(uint16_t reg)
{
    uint8_t read_buffer[3];            // Buffer para armazenar comando e resposta (3 bytes)
    uint8_t cmd = 0x20 | (reg & 0x1F); // Comando de leitura de registrador

    HAL_GPIO_WritePin(GPIOA, ADS1262_CS_Pin, GPIO_PIN_RESET); // Ativar chip select

    // Enviar comando de leitura
    HAL_SPI_Transmit(&hspi1, &cmd, 1, 10);
    HAL_SPI_Receive(&hspi1, read_buffer, 3, 10); // Ler 3 bytes: 1 byte de comando, 2 bytes de dados

    HAL_GPIO_WritePin(GPIOA, ADS1262_CS_Pin, GPIO_PIN_SET); // Desativar chip select

    //sprintf(charData, "%X \n ", read_buffer[1]);
    //CDC_Transmit_FS((uint8_t *)charData, strlen(charData));

    return read_buffer[1]; // Retornar o byte de dados lido
}

// Function to Read Data (ADC result)
uint32_t ADS1262_ReadData()
{
    uint8_t rxData[6]; // ADS1262 retorna 5 bytes: status + 4 bytes de ADC
    uint32_t adc_value = 0;

    while (HAL_GPIO_ReadPin(ADS1262_DRDY_GPIO_Port, ADS1262_DRDY_Pin) == GPIO_PIN_SET);

    ADS1262_Select();

    HAL_SPI_Receive(&hspi1, rxData, 6, 10); // Lê os 5 bytes do ADS1262

    ADS1262_Deselect();

    // Monta o valor de 32 bits (ignorando o primeiro byte de status)
    adc_value = ((uint32_t)rxData[1] << 24) | ((uint32_t)rxData[2] << 16) |
                ((uint32_t)rxData[3] << 8) | (uint32_t)rxData[4];

    return adc_value;
}
// Function to Initialize ADS1262
void ADS1262_Init()
{
    HAL_GPIO_WritePin(ADS1262_RESET_GPIO_Port, ADS1262_RESET_Pin, GPIO_PIN_SET); // Reset high
    HAL_Delay(10);
    HAL_GPIO_WritePin(ADS1262_RESET_GPIO_Port, ADS1262_RESET_Pin, GPIO_PIN_RESET); // Reset low
    HAL_Delay(10);
    HAL_GPIO_WritePin(ADS1262_RESET_GPIO_Port, ADS1262_RESET_Pin, GPIO_PIN_SET); // Reset high again
    // ADS1262_HARD_STOP();
    HAL_Delay(100);
    ADS1262_START();

    // Set SPI speed, ADC mode, gain, etc.
    ADS1262_WriteRegister(0x01, 0x11);  // Set Power Mode Register
        HAL_Delay(10);
        ADS1262_WriteRegister(0x02, 0x05);	// INTERFACE
        HAL_Delay(10);
        ADS1262_WriteRegister(0x03, 0x00);	// MODE0
        HAL_Delay(10);
        ADS1262_WriteRegister(0x04, 0x80);	// MODE1
        HAL_Delay(10);
        ADS1262_WriteRegister(0x05, 0x04);	// MODE2
        HAL_Delay(10);
        ADS1262_WriteRegister(0x06, 0x6A);	// INPMUX
        HAL_Delay(10);
}

void ADS1262_setPGA(uint8_t PGA)
{
    ADS1262_WriteRegister(MODE2, PGA | 0x04); // 0x04 sets the 20 SPS
}

void ADS1262_setAIN(uint16_t AINP, uint16_t AINN)
{
    ADS1262_WriteRegister(0x06, (uint8_t)(((AINP & 0x0F) << 4) | (AINN & 0x0F)));
}

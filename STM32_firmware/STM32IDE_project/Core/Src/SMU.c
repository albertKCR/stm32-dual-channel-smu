#include "SMU.h"

void SMU_Init(SPI_HandleTypeDef DAC8760hspi)
{
    Channel1_Init(&Channel1, &Ch1_Potentiostat_DAC, &Ch1_Galvanostat_DAC, AIN0, AIN4, AINCOM, DAC8760hspi);
    Channel2_Init(&Channel2, &Ch2_Potentiostat_DAC, &Ch2_Galvanostat_DAC, AIN2, AIN6, AINCOM, DAC8760hspi);

    ADS1262_Init();
}

void Channel1_Init(channel_t *channel, DAC8760_t *potentiostat_DAC, DAC8760_t *galvanostat_DAC, uint16_t ammeter_AIN,
                  uint16_t voltmeter_AINP, uint16_t voltmeter_AINN, SPI_HandleTypeDef DAC8760hspi)
{
    DAC8760_Init(&Ch1_Galvanostat_DAC, DAC8760_1_LATCH_Pin, DAC8760_1_LATCH_GPIO_Port, DAC8760hspi);

    channel->Potentiostat_DAC = *potentiostat_DAC;
    channel->Galvanostat_DAC = *galvanostat_DAC;
    channel->Ammeter_AIN = ammeter_AIN;
    channel->Voltmeter_AINN = voltmeter_AINN;
    channel->Voltmeter_AINP = voltmeter_AINP;
    channel->channel1 = 1;
}

void Channel2_Init(channel_t *channel, DAC8760_t *potentiostat_DAC, DAC8760_t *galvanostat_DAC, uint16_t ammeter_AIN,
                  uint16_t voltmeter_AINP, uint16_t voltmeter_AINN, SPI_HandleTypeDef DAC8760hspi)
{
    DAC8760_Init(&Ch2_Potentiostat_DAC, DAC8760_3_LATCH_Pin, DAC8760_3_LATCH_GPIO_Port, DAC8760hspi);

    channel->Potentiostat_DAC = *potentiostat_DAC;
    channel->Galvanostat_DAC = *galvanostat_DAC;
    channel->Ammeter_AIN = ammeter_AIN;
    channel->Voltmeter_AINN = voltmeter_AINN;
    channel->Voltmeter_AINP = voltmeter_AINP;
    channel->channel1 = 0;
}

double SMU_VoltageRead(channel_t channel)
{
    int32_t adc_data = 0;
    double voltage = 0;
    ADS1262_setAIN(channel.Voltmeter_AINP, channel.Voltmeter_AINN);
    adc_data = ADS1262_ReadData();
    voltage = (double)adc_data * (2.5 / pow(2, 31));

    return voltage * 4;
}

double SMU_CurrentRead(channel_t channel)
{
    int32_t adc_data = 0;
    double voltage = 0;
    double current = 0;

    ADS1262_setAIN(AIN0, AINCOM);
    adc_data = ADS1262_ReadData();

    voltage = (double)adc_data * (2.5 / pow(2, 31));
    while ((voltage > 2.4 || voltage < -2.4) || (voltage > -0.1 && voltage < 0.1))
    {
    	if((voltage > 2.4 || voltage < -2.4) && currentAmmeterResistorIndex != 0)
    	{
    		currentAmmeterResistorIndex--;
    	}
    	else if ((voltage > -0.1 && voltage < 0.1) && currentAmmeterResistorIndex != 6)
    	{
    		currentAmmeterResistorIndex++;
    	}
    	currentAmmeterResistor = ammeterResistors[currentAmmeterResistorIndex];

    	SMU_setAmmeterResistor(currentAmmeterResistorIndex, channel);

    	adc_data = ADS1262_ReadData();
        voltage = (double)adc_data * (2.5 / pow(2, 31));
    }

    current = voltage / currentAmmeterResistor;

    current = voltage / 1000;

    return current*(-1);
}


void SMU_setAmmeterResistor(int currentAmmeterResistorIndex, channel_t channel)
{
	char charData[15];
	switch (currentAmmeterResistorIndex) {
		case 0:
			HAL_GPIO_WritePin(ADG1408_1_A0_GPIO_Port, ADG1408_1_A0_Pin, GPIO_PIN_RESET);
			HAL_GPIO_WritePin(ADG1408_1_A1_GPIO_Port, ADG1408_1_A1_Pin, GPIO_PIN_RESET);
			HAL_GPIO_WritePin(ADG1408_1_A2_GPIO_Port, ADG1408_1_A2_Pin, GPIO_PIN_RESET);
			break;
		case 1:
            HAL_GPIO_WritePin(ADG1408_1_A0_GPIO_Port, ADG1408_1_A0_Pin, GPIO_PIN_SET);
			HAL_GPIO_WritePin(ADG1408_1_A1_GPIO_Port, ADG1408_1_A1_Pin, GPIO_PIN_RESET);
			HAL_GPIO_WritePin(ADG1408_1_A2_GPIO_Port, ADG1408_1_A2_Pin, GPIO_PIN_RESET);
			break;
		case 2:
            HAL_GPIO_WritePin(ADG1408_1_A0_GPIO_Port, ADG1408_1_A0_Pin, GPIO_PIN_RESET);
			HAL_GPIO_WritePin(ADG1408_1_A1_GPIO_Port, ADG1408_1_A1_Pin, GPIO_PIN_SET);
			HAL_GPIO_WritePin(ADG1408_1_A2_GPIO_Port, ADG1408_1_A2_Pin, GPIO_PIN_RESET);
			break;
		case 3:
            HAL_GPIO_WritePin(ADG1408_1_A0_GPIO_Port, ADG1408_1_A0_Pin, GPIO_PIN_SET);
			HAL_GPIO_WritePin(ADG1408_1_A1_GPIO_Port, ADG1408_1_A1_Pin, GPIO_PIN_SET);
			HAL_GPIO_WritePin(ADG1408_1_A2_GPIO_Port, ADG1408_1_A2_Pin, GPIO_PIN_RESET);
			break;
        case 4:
            HAL_GPIO_WritePin(ADG1408_1_A0_GPIO_Port, ADG1408_1_A0_Pin, GPIO_PIN_RESET);
			HAL_GPIO_WritePin(ADG1408_1_A1_GPIO_Port, ADG1408_1_A1_Pin, GPIO_PIN_RESET);
			HAL_GPIO_WritePin(ADG1408_1_A2_GPIO_Port, ADG1408_1_A2_Pin, GPIO_PIN_SET);
			break;
        case 5:
            HAL_GPIO_WritePin(ADG1408_1_A0_GPIO_Port, ADG1408_1_A0_Pin, GPIO_PIN_SET);
			HAL_GPIO_WritePin(ADG1408_1_A1_GPIO_Port, ADG1408_1_A1_Pin, GPIO_PIN_RESET);
			HAL_GPIO_WritePin(ADG1408_1_A2_GPIO_Port, ADG1408_1_A2_Pin, GPIO_PIN_SET);
			break;
        case 6:
            HAL_GPIO_WritePin(ADG1408_1_A0_GPIO_Port, ADG1408_1_A0_Pin, GPIO_PIN_RESET);
			HAL_GPIO_WritePin(ADG1408_1_A1_GPIO_Port, ADG1408_1_A1_Pin, GPIO_PIN_SET);
			HAL_GPIO_WritePin(ADG1408_1_A2_GPIO_Port, ADG1408_1_A2_Pin, GPIO_PIN_SET);
			break;
	}
}


void SMU_SetVoltage(float output, channel_t channel)
{
    uint16_t output16 = ceil(((output + 10) / (20)) * 65535);
    DAC8760_WriteVoltage(&channel.Potentiostat_DAC, output16);
}

void SMU_SetCurrent(float output, channel_t channel)
{
	float voltageOutput = output * currentSourceResistor;
    uint16_t output16 = ceil(((voltageOutput + 10) / (20)) * 65535);
    DAC8760_WriteVoltage(&channel.Galvanostat_DAC, output16);
}

void SMU_LSV(LSV lsv)
{
	char charData[15];
    pageID = 0x0B;
    setDisplayPage(pageID);
    double read;
    char message[200];
    int stepTime = (lsv.voltageStep * 1000 * 1000) / lsv.scanRate;
    uint32_t timer = 0;

    int32_t adc_data = 0;
    double voltage = 0;

    int direction;

    float i = lsv.initialVoltage;

    if (lsv.finalVoltage > lsv.initialVoltage)
        direction = 1;
    else
        direction = -1;

    while ((direction == 1 && i <= lsv.finalVoltage) || (direction == -1 && i >= lsv.finalVoltage))
    {
        SMU_SetVoltage(i, lsv.channel);

        timer = __HAL_TIM_GET_COUNTER(&htim2);
        while (((__HAL_TIM_GET_COUNTER(&htim2) - timer)) < (stepTime*1000)) // TIM2 resolution is in us
        {
        }

        read = SMU_CurrentRead(lsv.channel);

        snprintf(message, sizeof(message), "p;%.5lf;%.13lf", i, read);
        CDC_Transmit_FS(message, strlen((char *)message));

        dwinSendDouble(0x2240, i);
        dwinSendDouble(0x2260, read);


        i += direction * lsv.voltageStep;
    }
    SMU_SetVoltage(0, lsv.channel);
    pageID = 0x00;
    setDisplayPage(pageID);
}

void SMU_CV(CV cv)
{
    pageID = 0x0B;
    setDisplayPage(pageID);
    double read;
    char message[200];
    float stepTime = (cv.voltageStep * 1000 * 1000) / cv.scanRate;
    uint32_t timer = 0;

    int direction;
    float i = cv.initialVoltage;

    if (cv.peak1Voltage > cv.initialVoltage)
        direction = 1;
    else
        direction = -1;

    for (int cycle = 0; cycle < cv.cycles; cycle++)
    {
        i = cv.initialVoltage;
        while ((direction == 1 && i <= cv.peak1Voltage) || (direction == -1 && i >= cv.peak1Voltage))
        {
            SMU_SetVoltage(i, cv.channel);

            timer = __HAL_TIM_GET_COUNTER(&htim2);
            while (((__HAL_TIM_GET_COUNTER(&htim2) - timer)) < (stepTime*1000)) // TIM2 resolution is in us
            {
            }

            read = SMU_CurrentRead(cv.channel);

            dwinSendDouble(0x2240, i);
            dwinSendDouble(0x2260, read);
            snprintf(message, sizeof(message), "p;%.5f;%.13f", i, read);
            CDC_Transmit_FS(message, strlen((char *)message));

            i += direction * cv.voltageStep;
        }

        if (cv.peak2Voltage > cv.peak1Voltage)
            direction = 1;
        else
            direction = -1;

        while ((direction == 1 && i <= cv.peak2Voltage) || (direction == -1 && i >= cv.peak2Voltage))
        {
            SMU_SetVoltage(i, cv.channel);

            timer = __HAL_TIM_GET_COUNTER(&htim2);
            while (((__HAL_TIM_GET_COUNTER(&htim2) - timer)) < (stepTime*1000)) // TIM2 resolution is in us
            {
            }

            read = SMU_CurrentRead(cv.channel);

            dwinSendDouble(0x2240, i);
            dwinSendDouble(0x2260, read);

            snprintf(message, sizeof(message), "p;%.5f;%.13f", i, read);
            CDC_Transmit_FS(message, strlen((char *)message));

            i += direction * cv.voltageStep;
        }

        if (cv.finalVoltage > cv.peak2Voltage)
            direction = 1;
        else
            direction = -1;

        while ((direction == 1 && i <= cv.finalVoltage) || (direction == -1 && i >= cv.finalVoltage))
        {
            SMU_SetVoltage(i, cv.channel);

            timer = __HAL_TIM_GET_COUNTER(&htim2);
            while (((__HAL_TIM_GET_COUNTER(&htim2) - timer)) < (stepTime*1000)) // TIM2 resolution is in us
            {
            }

            read = SMU_CurrentRead(cv.channel);

            dwinSendDouble(0x2240, i);
            dwinSendDouble(0x2260, read);

            snprintf(message, sizeof(message), "p;%.5f;%.13f", i, read);
            CDC_Transmit_FS(message, strlen((char *)message));

            i += direction * cv.voltageStep;
        }
    }
    SMU_SetVoltage(0, cv.channel);
    pageID = 0x00;
    setDisplayPage(pageID);
}

void SMU_DPV(DPV dpv)
{
    pageID = 0x0B;
    setDisplayPage(pageID);
    double read1, read2;
    char message[200];
    uint32_t timer = 0;

    int direction;

    if (dpv.finalVoltage > dpv.initialVoltage)
        direction = 1;
    else
        direction = -1;

    float lastVoltage = dpv.initialVoltage;
    SMU_SetVoltage(lastVoltage, dpv.channel);
    SMU_CurrentRead(dpv.channel);

    while ((direction == 1 && lastVoltage <= dpv.finalVoltage) || (direction == -1 && lastVoltage >= dpv.finalVoltage))
    {
        lastVoltage += direction * (dpv.voltagePulse + dpv.voltageStep);

        SMU_SetVoltage(lastVoltage, dpv.channel);

        timer = __HAL_TIM_GET_COUNTER(&htim2);
        while (((__HAL_TIM_GET_COUNTER(&htim2) - timer)) < (dpv.pulseTime*1000)) // TIM2 resolution is in us
        {
        }

        read1 = SMU_CurrentRead(dpv.channel);

        lastVoltage -= direction * dpv.voltagePulse;

        SMU_SetVoltage(lastVoltage, dpv.channel);

        timer = __HAL_TIM_GET_COUNTER(&htim2);
        while (((__HAL_TIM_GET_COUNTER(&htim2) - timer)) < (dpv.baseTime*1000)) // TIM2 resolution is in us
        {
        }

        read2 = SMU_CurrentRead(dpv.channel);

        dwinSendDouble(0x2240, lastVoltage + direction * dpv.voltagePulse);
        dwinSendDouble(0x2260, read1 - read2);

        snprintf(message, sizeof(message), "p;%.5f;%.13f", lastVoltage + direction * dpv.voltagePulse, read1 - read2);
        CDC_Transmit_FS(message, strlen((char *)message));
    }

    SMU_SetVoltage(0, dpv.channel);
    pageID = 0x00;
    setDisplayPage(pageID);
}

void SMU_SWV(SWV swv)
{
    pageID = 0x0B;
    setDisplayPage(pageID);
    double read1, read2;
    char message[200];
    float stepTime = 1000 / swv.frequency;
    uint32_t timer = 0;

    int direction;

    if (swv.finalVoltage > swv.initialVoltage)
        direction = 1;
    else
        direction = -1;

    float lastVoltage = swv.initialVoltage;
    SMU_SetVoltage(lastVoltage, swv.channel);
    read1 = SMU_CurrentRead(swv.channel);

    dwinSendDouble(0x2240, lastVoltage);
    dwinSendDouble(0x2260, read1);

    snprintf(message, sizeof(message), "p;%.5f;%.13f", lastVoltage, read1);
    CDC_Transmit_FS(message, strlen((char *)message));

    while ((direction == 1 && lastVoltage <= swv.finalVoltage) || (direction == -1 && lastVoltage >= swv.finalVoltage))
    {
        lastVoltage += direction * (swv.voltageAmplitude + swv.voltageStep);

        SMU_SetVoltage(lastVoltage, swv.channel);

        timer = __HAL_TIM_GET_COUNTER(&htim2);
        while (((__HAL_TIM_GET_COUNTER(&htim2) - timer)) < (stepTime*1000/2)) // TIM2 resolution is in us
        {
        }

        read1 = SMU_CurrentRead(swv.channel);

        lastVoltage -= direction * swv.voltageAmplitude;

        SMU_SetVoltage(lastVoltage, swv.channel);

        timer = __HAL_TIM_GET_COUNTER(&htim2);
        while (((__HAL_TIM_GET_COUNTER(&htim2) - timer)) < (stepTime*1000/2)) // TIM2 resolution is in us
        {
        }

        read2 = SMU_CurrentRead(swv.channel);

        dwinSendDouble(0x2240, lastVoltage + direction * swv.voltageAmplitude);
        dwinSendDouble(0x2260, read1 - read2);

        snprintf(message, sizeof(message), "p;%.5f;%.13f", lastVoltage + direction * swv.voltageAmplitude, read1 - read2);
        CDC_Transmit_FS(message, strlen((char *)message));
    }
    SMU_SetVoltage(0, swv.channel);
    pageID = 0x00;
    setDisplayPage(pageID);
}

void SMU_CP(CP cp)
{
    pageID = 0x0A;
    setDisplayPage(pageID);
    double read;
    char message[200];
    uint32_t samplePeriod = cp.samplePeriod * 1000;
    uint32_t sampleTime = cp.sampleTime;
    uint32_t timer1 = 0;
    uint32_t timer2 = 0;

    int direction;

    SMU_SetCurrent(cp.constCurrent, cp.channel);

    timer1 = __HAL_TIM_GET_COUNTER(&htim2);
    while ((__HAL_TIM_GET_COUNTER(&htim2) - timer1) < samplePeriod*1000)
    {
        timer2 = __HAL_TIM_GET_COUNTER(&htim2);
        while (((__HAL_TIM_GET_COUNTER(&htim2) - timer2)) < (sampleTime*1000)) // TIM2 resolution is in us
        {

        }

        read = SMU_VoltageRead(cp.channel);

        dwinSendDouble(0x2200, cp.constCurrent);
        dwinSendDouble(0x2220, read);

        snprintf(message, sizeof(message), "g;%.5f;%.5f", cp.constCurrent, read);
        CDC_Transmit_FS(message, strlen((char *)message));
    }
    SMU_SetCurrent(0, cp.channel);
    pageID = 0x00;
    setDisplayPage(pageID);
}

void SMU_LSP(LSP lsp)
{
    pageID = 0x0A;
    setDisplayPage(pageID);
    double read;
    char message[200];
    float stepTime = (lsp.currentStep * 1000 * 1000 * 1000) / lsp.scanRate;
    uint32_t timer = 0;

    int direction;

    float i = lsp.initialCurrent;

    if (lsp.finalCurrent > lsp.initialCurrent)
        direction = 1;
    else
        direction = -1;

    SMU_SetCurrent(i, lsp.channel);
    read = SMU_VoltageRead(lsp.channel);
    while ((direction == 1 && i <= lsp.finalCurrent) || (direction == -1 && i >= lsp.finalCurrent))
    {
        SMU_SetCurrent(i, lsp.channel);

        timer = __HAL_TIM_GET_COUNTER(&htim2);
        while (((__HAL_TIM_GET_COUNTER(&htim2) - timer)) < (stepTime*1000)) // TIM2 resolution is in us
        {

        }
        read = SMU_VoltageRead(lsp.channel);

        dwinSendDouble(0x2200, i);
        dwinSendDouble(0x2220, read);
        snprintf(message, sizeof(message), "g;%.9lf;%.9lf", i, read-0.013);
        CDC_Transmit_FS(message, strlen((char *)message));

        i += direction * lsp.currentStep;
    }
    SMU_SetCurrent(0, lsp.channel);
    pageID = 0x00;
    setDisplayPage(pageID);
}

void SMU_CyP(CyP cyp)
{
    pageID = 0x0A;
    setDisplayPage(pageID);
    double read;
    char message[200];
    float stepTime = (cyp.currentStep * 1000 * 1000) / cyp.scanRate;
    uint32_t timer = 0;

    int direction;
    float i = cyp.initialCurrent;

    if (cyp.peak1Current > cyp.initialCurrent)
        direction = 1;
    else
        direction = -1;

    for (int cycle = 0; cycle < cyp.cycles; cycle++)
    {
        i = cyp.initialCurrent;
        while ((direction == 1 && i <= cyp.peak1Current) || (direction == -1 && i >= cyp.peak1Current))
        {
            SMU_SetCurrent(i, cyp.channel);

            timer = __HAL_TIM_GET_COUNTER(&htim2);
            while (((__HAL_TIM_GET_COUNTER(&htim2) - timer)) < (stepTime*1000)) // TIM2 resolution is in us
            {
            }

            read = SMU_VoltageRead(cyp.channel);

            dwinSendDouble(0x2200, i);
            dwinSendDouble(0x2220, read);

            snprintf(message, sizeof(message), "g;%.5f;%.5f", i, read);
            CDC_Transmit_FS(message, strlen((char *)message));

            i += direction * cyp.currentStep;
        }

        if (cyp.peak2Current > cyp.peak1Current)
            direction = 1;
        else
            direction = -1;

        while ((direction == 1 && i <= cyp.peak2Current) || (direction == -1 && i >= cyp.peak2Current))
        {
            SMU_SetCurrent(i, cyp.channel);

            timer = __HAL_TIM_GET_COUNTER(&htim2);
            while (((__HAL_TIM_GET_COUNTER(&htim2) - timer)) < (stepTime*1000)) // TIM2 resolution is in us
            {
            }

            read = SMU_VoltageRead(cyp.channel);

            dwinSendDouble(0x2200, i);
            dwinSendDouble(0x2220, read);

            snprintf(message, sizeof(message), "g;%.5f;%.5f", i, read);
            CDC_Transmit_FS(message, strlen((char *)message));

            i += direction * cyp.currentStep;
        }

        if (cyp.finalCurrent > cyp.peak2Current)
            direction = 1;
        else
            direction = -1;

        while ((direction == 1 && i <= cyp.finalCurrent) || (direction == -1 && i >= cyp.finalCurrent))
        {
            SMU_SetCurrent(i, cyp.channel);

            timer = __HAL_TIM_GET_COUNTER(&htim2);
            while (((__HAL_TIM_GET_COUNTER(&htim2) - timer)) < (stepTime*1000)) // TIM2 resolution is in us
            {
            }

            read = SMU_VoltageRead(cyp.channel);

            dwinSendDouble(0x2200, i);
            dwinSendDouble(0x2220, read);

            snprintf(message, sizeof(message), "g;%.5f;%.5f", i, read);
            CDC_Transmit_FS(message, strlen((char *)message));

            i += direction * cyp.currentStep;
        }
    }
    SMU_SetCurrent(0, cyp.channel);
    pageID = 0x00;
    setDisplayPage(pageID);
}

void SMU_dualChannelMeasure(ArrayMeasurementData channel1Data, ArrayMeasurementData channel2Data)
{
    int j = 0;
    int i = 0;
    uint32_t timerCh1 = 0;
    uint32_t timerCh2 = 0;
    char message[200];
    double read;

    while ((j < channel1Data.length) || (i < channel2Data.length))
    {
        if (((HAL_GetTick() - timerCh1) > channel1Data.timeStep) && (j < channel1Data.length))
        {
            if (channel1Data.potentiostat)
            {
                SMU_SetVoltage(channel1Data.values[j], channel1Data.channel);
                read = SMU_CurrentRead(channel1Data.channel);
                snprintf(message, sizeof(message), "ch1;p;%.5f;%.13f", channel1Data.values[j], read);
                CDC_Transmit_FS(message, strlen((char *)message));
                j++;
            }
            else
            {
                SMU_SetCurrent(channel1Data.values[j], channel1Data.channel);
                read = SMU_VoltageRead(channel1Data.channel);
                snprintf(message, sizeof(message), "ch1;g;%.5f;%.5f", channel1Data.values[j], read);
                CDC_Transmit_FS(message, strlen((char *)message));
                j++;
            }
            timerCh1 = HAL_GetTick();
        }
        if (((HAL_GetTick() - timerCh2) > channel2Data.timeStep) && (i < channel2Data.length))
        {
            if (channel2Data.potentiostat)
            {
                SMU_SetVoltage(channel2Data.values[i], channel2Data.channel);
                read = SMU_CurrentRead(channel2Data.channel);
                snprintf(message, sizeof(message), "ch2;p;%.5f;%.13f", channel2Data.values[i], read);
                CDC_Transmit_FS(message, strlen((char *)message));
                i++;
            }
            else
            {
                SMU_SetCurrent(channel2Data.values[i], channel2Data.channel);
                read = SMU_VoltageRead(channel2Data.channel);
                snprintf(message, sizeof(message), "ch2;g;%.5f;%.5f", channel2Data.values[i], read); // g means galvanostat
                CDC_Transmit_FS(message, strlen((char *)message));
                i++;
            }
            timerCh2 = HAL_GetTick();
        }
    }
}

void dwin_interface()
{
    waitForDwin();
}

void waitForDwin()
{
    uint8_t buffer1[10];

    uint8_t rxByte[6] = {0};
    if (HAL_UART_Receive(&huart1, rxByte, 6, 500) == HAL_OK)
    {
        sprintf(buffer1, "%02X%02X", rxByte[4], rxByte[5]);
        CDC_Transmit_FS(buffer1, strlen(buffer1));

        if (strcmp(buffer1, "2500") == 0)
        {
            pageID = 0x01;
            dwinChannel = 1;
            setDisplayPage(pageID);
            waitForDwinTechnique();
        }
        else if (strcmp(buffer1, "2501") == 0)
        {
            pageID = 0x01;
            dwinChannel = 2;
            setDisplayPage(pageID);
            waitForDwinTechnique();
        }
    }
}

void waitForDwinTechnique()
{
    while (pageID == 0x01)
    {
        uint8_t msg[10];
        sprintf(msg, "pageID=0x01");
        CDC_Transmit_FS(msg, strlen(msg));

        uint8_t buffer1[10];

        uint8_t rxByte[6] = {0};
        if (HAL_UART_Receive(&huart1, rxByte, 6, 500) == HAL_OK)
        {
            sprintf(buffer1, "%02X%02X", rxByte[4], rxByte[5]);
            CDC_Transmit_FS(buffer1, strlen(buffer1));

            if (strcmp(buffer1, "2510") == 0) // LSV
            {
                pageID = 0x02;
                setDisplayPage(pageID);
                LSV_DWIN_Page();
            }
            if (strcmp(buffer1, "2511") == 0) // CV
            {
                pageID = 0x03;
                setDisplayPage(pageID);
                CV_DWIN_Page();
            }
            if (strcmp(buffer1, "2512") == 0) // DPV
            {
                pageID = 0x04;
                setDisplayPage(pageID);
                DPV_DWIN_Page();
            }
            if (strcmp(buffer1, "2513") == 0) // SWV
            {
                pageID = 0x05;
                setDisplayPage(pageID);
                SWV_DWIN_Page();
            }

            if (strcmp(buffer1, "2514") == 0) // CP
            {
                pageID = 0x06;
                setDisplayPage(pageID);
                CP_DWIN_Page();
            }
            if (strcmp(buffer1, "2515") == 0) // LSP
            {
                pageID = 0x07;
                setDisplayPage(pageID);
                LSP_DWIN_Page();
            }
            if (strcmp(buffer1, "2516") == 0) // CyP
            {
                pageID = 0x08;
                setDisplayPage(pageID);
                CyP_DWIN_Page();
            }
            if (strcmp(buffer1, "2517") == 0) // Back button
            {
                pageID = 0x00;
                dwinChannel = 0;
                setDisplayPage(pageID);
            }
        }
    }
}

void LSV_DWIN_Page()
{
    while (pageID == 0x02)
    {
        uint8_t buffer1[10];

        uint8_t rxByte[6] = {0};
        if (HAL_UART_Receive(&huart1, rxByte, 6, 500) == HAL_OK)
        {
            sprintf(buffer1, "%02X%02X", rxByte[4], rxByte[5]);
            CDC_Transmit_FS(buffer1, strlen(buffer1));

            if (strcmp(buffer1, "2518") == 0) // Back button
            {
                pageID = 0x01;
                setDisplayPage(pageID);
            }

            if (strcmp(buffer1, "2519") == 0) // Start button
            {
                getLSVParameters_DWIN();
            }
        }
    }
}

void CV_DWIN_Page()
{
    while (pageID == 0x03)
    {
        uint8_t buffer1[10];

        uint8_t rxByte[6] = {0};
        if (HAL_UART_Receive(&huart1, rxByte, 6, 500) == HAL_OK)
        {
            sprintf(buffer1, "%02X%02X", rxByte[4], rxByte[5]);
            CDC_Transmit_FS(buffer1, strlen(buffer1));

            if (strcmp(buffer1, "2518") == 0) // Back button
            {
                pageID = 0x01;
                setDisplayPage(pageID);
            }

            if (strcmp(buffer1, "2519") == 0) // Start button
            {
                getCVParameters_DWIN();
            }
        }
    }
}

void DPV_DWIN_Page()
{
    while (pageID == 0x04)
    {
        uint8_t buffer1[10];

        uint8_t rxByte[6] = {0};
        if (HAL_UART_Receive(&huart1, rxByte, 6, 500) == HAL_OK)
        {
            sprintf(buffer1, "%02X%02X", rxByte[4], rxByte[5]);
            CDC_Transmit_FS(buffer1, strlen(buffer1));

            if (strcmp(buffer1, "2518") == 0) // Back button
            {
                pageID = 0x01;
                setDisplayPage(pageID);
            }

            if (strcmp(buffer1, "2519") == 0) // Start button
            {
                getDPVParameters_DWIN();
            }
        }
    }
}

void SWV_DWIN_Page()
{
    while (pageID == 0x05)
    {
        uint8_t buffer1[10];

        uint8_t rxByte[6] = {0};
        if (HAL_UART_Receive(&huart1, rxByte, 6, 500) == HAL_OK)
        {
            sprintf(buffer1, "%02X%02X", rxByte[4], rxByte[5]);
            CDC_Transmit_FS(buffer1, strlen(buffer1));

            if (strcmp(buffer1, "2518") == 0) // Back button
            {
                pageID = 0x01;
                setDisplayPage(pageID);
            }

            if (strcmp(buffer1, "2519") == 0) // Start button
            {
                getSWVParameters_DWIN();
            }
        }
    }
}

void CP_DWIN_Page()
{
    while (pageID == 0x06)
    {
        uint8_t buffer1[10];

        uint8_t rxByte[6] = {0};
        if (HAL_UART_Receive(&huart1, rxByte, 6, 500) == HAL_OK)
        {
            sprintf(buffer1, "%02X%02X", rxByte[4], rxByte[5]);
            CDC_Transmit_FS(buffer1, strlen(buffer1));

            if (strcmp(buffer1, "2518") == 0) // Back button
            {
                pageID = 0x01;
                setDisplayPage(pageID);
            }

            if (strcmp(buffer1, "2519") == 0) // Start button
            {
                getCPParameters_DWIN();
            }
        }
    }
}

void LSP_DWIN_Page()
{
    while (pageID == 0x07)
    {
        uint8_t buffer1[10];

        uint8_t rxByte[6] = {0};
        if (HAL_UART_Receive(&huart1, rxByte, 6, 500) == HAL_OK)
        {
            sprintf(buffer1, "%02X%02X", rxByte[4], rxByte[5]);
            CDC_Transmit_FS(buffer1, strlen(buffer1));

            if (strcmp(buffer1, "2518") == 0) // Back button
            {
                pageID = 0x01;
                setDisplayPage(pageID);
            }

            if (strcmp(buffer1, "2519") == 0) // Start button
            {
                getLSPParameters_DWIN();
            }
        }
    }
}

void CyP_DWIN_Page()
{
    while (pageID == 0x08)
    {
        uint8_t buffer1[10];

        uint8_t rxByte[6] = {0};
        if (HAL_UART_Receive(&huart1, rxByte, 6, 500) == HAL_OK)
        {
            sprintf(buffer1, "%02X%02X", rxByte[4], rxByte[5]);
            CDC_Transmit_FS(buffer1, strlen(buffer1));

            if (strcmp(buffer1, "2518") == 0) // Back button
            {
                pageID = 0x01;
                setDisplayPage(pageID);
            }

            if (strcmp(buffer1, "2519") == 0) // Start button
            {
                getCyPParameters_DWIN();
            }
        }
    }
}

void getLSVParameters_DWIN()
{
    LSV lsv;

    if (dwinChannel == 1) lsv.channel = Channel1;
    else if (dwinChannel == 2) lsv.channel = Channel2;

    readVP(LSV_InitialVoltage_VP, 6);
    readDisplayFloat(&lsv.initialVoltage);

    readVP(LSV_FinalVoltage_VP, 6);
    readDisplayFloat(&lsv.finalVoltage);

    readVP(LSV_StepSize_VP, 6);
    readDisplayFloat(&lsv.voltageStep);

    readVP(LSV_ScanRate_VP, 6);
    readDisplayFloat(&lsv.scanRate);

    uint8_t buffer1[10];
    sprintf(buffer1, "%f; %f; %f; %f", lsv.initialVoltage, lsv.finalVoltage, lsv.voltageStep, lsv.scanRate);
    CDC_Transmit_FS(buffer1, strlen(buffer1));

    SMU_LSV(lsv);
}

void getCVParameters_DWIN()
{
    CV cv;

    if (dwinChannel == 1) cv.channel = Channel1;
    else if (dwinChannel == 2) cv.channel = Channel2;

    readVP(CV_InitialVoltage_VP, 6);
    readDisplayFloat(&cv.initialVoltage);

    readVP(CV_FinalVoltage_VP, 6);
    readDisplayFloat(&cv.finalVoltage);

    readVP(CV_Peak1_VP, 6);
    readDisplayFloat(&cv.peak1Voltage);

    readVP(CV_Peak2_VP, 6);
    readDisplayFloat(&cv.peak2Voltage);

    readVP(CV_StepSize_VP, 6);
    readDisplayFloat(&cv.voltageStep);

    readVP(CV_ScanRate_VP, 6);
    readDisplayFloat(&cv.scanRate);

    readVP(CV_Cycles_VP, 6);
    readDisplayFloat(&cv.cycles);

    uint8_t buffer1[10];
    sprintf(buffer1, "%f; %f; %f; %f; %f; %f; %f", cv.initialVoltage, cv.finalVoltage, cv.peak1Voltage, cv.peak2Voltage, cv.voltageStep, cv.scanRate, cv.cycles);
    CDC_Transmit_FS(buffer1, strlen(buffer1));

    SMU_CV(cv);
}

void getDPVParameters_DWIN()
{
    DPV dpv;

    if (dwinChannel == 1) dpv.channel = Channel1;
    else if (dwinChannel == 2) dpv.channel = Channel2;

    readVP(DPV_InitialVoltage_VP, 6);
    readDisplayFloat(&dpv.initialVoltage);

    readVP(DPV_FinalVoltage_VP, 6);
    readDisplayFloat(&dpv.finalVoltage);

    readVP(DPV_StepSize_VP, 6);
    readDisplayFloat(&dpv.voltageStep);

    readVP(DPV_PulseV_VP, 6);
    readDisplayFloat(&dpv.voltagePulse);

    readVP(DPV_PulseTime_VP, 6);
    readDisplayFloat(&dpv.pulseTime);

    readVP(DPV_BaseTime_VP, 6);
    readDisplayFloat(&dpv.baseTime);

    uint8_t buffer1[10];
    sprintf(buffer1, "%f; %f; %f; %f; %f; %f", dpv.initialVoltage, dpv.finalVoltage, dpv.voltageStep, dpv.voltagePulse, dpv.pulseTime, dpv.baseTime);
    CDC_Transmit_FS(buffer1, strlen(buffer1));

    SMU_DPV(dpv);
}

void getSWVParameters_DWIN()
{
    SWV swv;

    if (dwinChannel == 1) swv.channel = Channel1;
    else if (dwinChannel == 2) swv.channel = Channel2;

    readVP(SWV_InitialVoltage_VP, 6);
    readDisplayFloat(&swv.initialVoltage);

    readVP(SWV_FinalVoltage_VP, 6);
    readDisplayFloat(&swv.finalVoltage);

    readVP(SWV_StepSize_VP, 6);
    readDisplayFloat(&swv.voltageStep);

    readVP(SWV_AmplitudeV_VP, 6);
    readDisplayFloat(&swv.voltageAmplitude);

    readVP(SWV_Frequency_VP, 6);
    readDisplayFloat(&swv.frequency);

    uint8_t buffer1[10];
    sprintf(buffer1, "%f; %f; %f; %f; %f", swv.initialVoltage, swv.finalVoltage, swv.voltageStep, swv.voltageAmplitude, swv.frequency);
    CDC_Transmit_FS(buffer1, strlen(buffer1));

    SMU_SWV(swv);
}

void getCPParameters_DWIN()
{
    CP cp;

    if (dwinChannel == 1) cp.channel = Channel1;
    else if (dwinChannel == 2) cp.channel = Channel2;

    readVP(CP_ConstI_VP, 6);
    readDisplayFloat(&cp.constCurrent);

    readVP(CP_SampleT_VP, 6);
    readDisplayFloat(&cp.sampleTime);

    readVP(CP_SampleP_VP, 6);
    readDisplayFloat(&cp.samplePeriod);

    uint8_t buffer1[10];
    sprintf(buffer1, "%f; %f; %f", cp.constCurrent, cp.sampleTime, cp.samplePeriod);
    CDC_Transmit_FS(buffer1, strlen(buffer1));

    SMU_CP(cp);
}

void getLSPParameters_DWIN()
{
    LSP lsp;

    if (dwinChannel == 1) lsp.channel = Channel1;
    else if (dwinChannel == 2) lsp.channel = Channel2;

    readVP(LSP_InitialI_VP, 6);
    readDisplayFloat(&lsp.initialCurrent);

    readVP(LSP_FinalI_VP, 6);
    readDisplayFloat(&lsp.finalCurrent);

    readVP(LSP_StepSize_VP, 6);
    readDisplayFloat(&lsp.currentStep);

    readVP(LSP_ScanRate_VP, 6);
    readDisplayFloat(&lsp.scanRate);

    uint8_t buffer1[10];
    sprintf(buffer1, "%f; %f; %f; %f", lsp.initialCurrent, lsp.finalCurrent, lsp.currentStep, lsp.scanRate);
    CDC_Transmit_FS(buffer1, strlen(buffer1));

    SMU_LSP(lsp);
}

void getCyPParameters_DWIN()
{
    CyP cyp;

    if (dwinChannel == 1) cyp.channel = Channel1;
    else if (dwinChannel == 2) cyp.channel = Channel2;

    readVP(CyP_InitialI_VP, 6);
    readDisplayFloat(&cyp.initialCurrent);

    readVP(CyP_FinalI_VP, 6);
    readDisplayFloat(&cyp.finalCurrent);

    readVP(CyP_Peak1_VP, 6);
    readDisplayFloat(&cyp.peak1Current);

    readVP(CyP_Peak2_VP, 6);
    readDisplayFloat(&cyp.peak2Current);

    readVP(CyP_StepSize_VP, 6);
    readDisplayFloat(&cyp.currentStep);

    readVP(CyP_ScanRate_VP, 6);
    readDisplayFloat(&cyp.scanRate);

    readVP(CyP_Cycles_VP, 6);
    readDisplayFloat(&cyp.cycles);

    uint8_t buffer1[10];
    sprintf(buffer1, "%f; %f; %f; %f; %f; %f; %f", cyp.initialCurrent, cyp.finalCurrent, cyp.peak1Current, cyp.peak2Current, cyp.currentStep, cyp.scanRate, cyp.cycles);
    CDC_Transmit_FS(buffer1, strlen(buffer1));

    SMU_CyP(cyp);
}

void pc_interface()
{
    bool channel1 = 0;
    bool channel2 = 0;
    //char input[] = ",2CyP;-5;5;-2;10;42;192;4";

    char *technique1;
    char *technique2;

    char channel1Parameters[sizeof(input)];
    strcpy(channel1Parameters, input);
    char channel2Parameters[sizeof(input)];
    strcpy(channel2Parameters, input);
    char *temp;

    if ((input[0] != ',') && (strstr(input, ",2") != NULL)) // verifica se tem ch1 e ch2
    {
        CDC_Transmit_FS("ch21", strlen((char *)"ch21"));
        channel1 = 1;
        channel2 = 1;
        temp = strtok(channel1Parameters, ",");
        temp = strtok(NULL, ",");
        strcpy(channel2Parameters, temp);
    }
    else if (strstr(input, ",2") != NULL) // verifica se tem só ch2
    {
        CDC_Transmit_FS("ch2", strlen((char *)"ch2"));
        channel2 = 1;
        strcpy(channel2Parameters, input);
        memmove(channel2Parameters, channel2Parameters + 1, strlen(channel2Parameters));
    }
    else // tem só canal1
    {
        CDC_Transmit_FS("ch1", strlen((char *)"ch1"));
        channel1 = 1;
    }

    ArrayMeasurementData channel1Data;
    // gets the ch1 technique
    HAL_Delay(100);
    if (channel1)
    {
        char technique1Buffer[sizeof(channel1Parameters)];
        strcpy(technique1Buffer, channel1Parameters);
        technique1 = strtok(technique1Buffer, ";");
        // direct the technique of channel 1
        if (strcmp(technique1, "LSV") == 0)
        {
            LSV lsv1;
            setLSVParameters(&lsv1, channel1Parameters, 0);
            if (!channel2)
                SMU_LSV(lsv1);
            else
            {
                channel1Data = BuildLSVarray(lsv1);
            }
        }
        else if (strcmp(technique1, "CV") == 0)
        {
            CV cv1;
            setCVParameters(&cv1, channel1Parameters, 0);
            if (!channel2)
                SMU_CV(cv1);
            else
                channel1Data = BuildCVarray(cv1);
        }
        else if (strcmp(technique1, "DPV") == 0)
        {
            DPV dpv1;
            setDPVParameters(&dpv1, channel1Parameters, 0);
            if (!channel2)
                SMU_DPV(dpv1);
        }
        else if (strcmp(technique1, "SWV") == 0)
        {
            SWV swv1;
            setSWVParameters(&swv1, channel1Parameters, 0);
            if (!channel2)
                SMU_SWV(swv1);
        }
        else if (strcmp(technique1, "CP") == 0)
        {
            CP cp1;
            setCPParameters(&cp1, channel1Parameters, 0);
            if (!channel2)
                SMU_CP(cp1);
            else
                channel1Data = BuildCParray(cp1);
        }
        else if (strcmp(technique1, "LSP") == 0)
        {
            LSP lsp1;
            setLSPParameters(&lsp1, channel1Parameters, 0);
            if (!channel2)
                SMU_LSP(lsp1);
            else
                channel1Data = BuildLSParray(lsp1);
        }
        else if (strcmp(technique1, "CyP") == 0)
        {
            CyP cyp1;
            setCyPParameters(&cyp1, channel1Parameters, 0);
            if (!channel2)
                SMU_CyP(cyp1);
            else
                channel1Data = BuildCyParray(cyp1);
        }
    }
    ArrayMeasurementData channel2Data;
    // gets the ch2 technique
    HAL_Delay(100);
    if (channel2)
    {
        char technique2Buffer[sizeof(channel2Parameters)];
        strcpy(technique2Buffer, channel2Parameters);
        technique2 = strtok(technique2Buffer, ";");
        // direct the technique of channel 2
        if (strcmp(technique2, "2LSV") == 0)
        {
            LSV lsv2;
            setLSVParameters(&lsv2, channel2Parameters, 1);
            if (!channel1)
                SMU_LSV(lsv2);
            else
                channel2Data = BuildLSVarray(lsv2);
        }
        else if (strcmp(technique2, "2CV") == 0)
        {
            CV cv2;
            setCVParameters(&cv2, channel2Parameters, 1);
            if (!channel1)
                SMU_CV(cv2);
            else
                channel2Data = BuildCVarray(cv2);
        }
        else if (strcmp(technique2, "2DPV") == 0)
        {
            DPV dpv2;
            setDPVParameters(&dpv2, channel2Parameters, 1);
            if (!channel1)
                SMU_DPV(dpv2);
        }
        else if (strcmp(technique2, "2SWV") == 0)
        {
            SWV swv2;
            setSWVParameters(&swv2, channel2Parameters, 1);
            if (!channel1)
                SMU_SWV(swv2);
        }
        else if (strcmp(technique2, "2CP") == 0)
        {
            CP cp2;
            setCPParameters(&cp2, channel2Parameters, 1);
            if (!channel1)
                SMU_CP(cp2);
            else
                channel2Data = BuildCParray(cp2);
        }
        else if (strcmp(technique2, "2LSP") == 0)
        {
            LSP lsp2;
            setLSPParameters(&lsp2, channel2Parameters, 1);
            if (!channel1)
                SMU_LSP(lsp2);
            else
                channel2Data = BuildLSParray(lsp2);
        }
        else if (strcmp(technique2, "2CyP") == 0)
        {
            CyP cyp2;
            setCyPParameters(&cyp2, channel2Parameters, 1);
            if (!channel1)
                SMU_CyP(cyp2);
            else
                channel2Data = BuildCyParray(cyp2);
        }
    }

    if (channel1 && channel2)
    {
        SMU_dualChannelMeasure(channel1Data, channel2Data);
    }
}

#pragma region Set Parameters
void setLSVParameters(LSV *lsv, char *parameters, bool channel) // channel=0 (channel 1), channel=1 (channel 2)
{
    if (!channel)
        lsv->channel = Channel1;
    else
        lsv->channel = Channel2;

    char *ParametersBuffer;

    ParametersBuffer = strtok(parameters, ";");

    ParametersBuffer = strtok(NULL, ";");
    lsv->initialVoltage = atof(ParametersBuffer);

    ParametersBuffer = strtok(NULL, ";");
    lsv->finalVoltage = atof(ParametersBuffer);

    ParametersBuffer = strtok(NULL, ";");
    lsv->voltageStep = atof(ParametersBuffer)/1000;

    ParametersBuffer = strtok(NULL, ";");
    lsv->scanRate = atof(ParametersBuffer);

    // char message[200];
    // snprintf(message, sizeof(message), "%f; %f; %f; %f;", lsv->initialVoltage, lsv->finalVoltage, lsv->scanRate, lsv->voltageStep);
    // CDC_Transmit_FS(message, strlen((char *)message));
}

void setCVParameters(CV *cv, char *parameters, bool channel)
{
    if (!channel)
        cv->channel = Channel1;
    else
        cv->channel = Channel2;

    char *ParametersBuffer;

    ParametersBuffer = strtok(parameters, ";");

    ParametersBuffer = strtok(NULL, ";");
    cv->initialVoltage = atof(ParametersBuffer);

    ParametersBuffer = strtok(NULL, ";");
    cv->peak1Voltage = atof(ParametersBuffer);

    ParametersBuffer = strtok(NULL, ";");
    cv->peak2Voltage = atof(ParametersBuffer);

    ParametersBuffer = strtok(NULL, ";");
    cv->finalVoltage = atof(ParametersBuffer);

    ParametersBuffer = strtok(NULL, ";");
    cv->voltageStep = atof(ParametersBuffer)/1000;

    ParametersBuffer = strtok(NULL, ";");
    cv->scanRate = atof(ParametersBuffer);

    ParametersBuffer = strtok(NULL, ";");
    cv->cycles = atof(ParametersBuffer);

    // char message[200];
//     snprintf(message, sizeof(message), "%f; %f; %f; %f; %f; %f; %f;", cv->initialVoltage, cv->peak1Voltage, cv->peak2Voltage
// , cv->finalVoltage, cv->voltageStep, cv->scanRate, cv->cycles);
//     CDC_Transmit_FS(message, strlen((char *)message));
}

void setDPVParameters(DPV *dpv, char *parameters, bool channel)
{
    if (!channel)
        dpv->channel = Channel1;
    else
        dpv->channel = Channel2;

    char *ParametersBuffer;

    ParametersBuffer = strtok(parameters, ";");

    ParametersBuffer = strtok(NULL, ";");
    dpv->initialVoltage = atof(ParametersBuffer);

    ParametersBuffer = strtok(NULL, ";");
    dpv->finalVoltage = atof(ParametersBuffer);

    ParametersBuffer = strtok(NULL, ";");
    dpv->voltageStep = atof(ParametersBuffer)/1000;

    ParametersBuffer = strtok(NULL, ";");
    dpv->voltagePulse = atof(ParametersBuffer)/1000;

    ParametersBuffer = strtok(NULL, ";");
    dpv->pulseTime = atof(ParametersBuffer);

    ParametersBuffer = strtok(NULL, ";");
    dpv->baseTime = atof(ParametersBuffer);

//     char message[200];
//     snprintf(message, sizeof(message), "%f; %f; %f; %f; %f; %f;", dpv->initialVoltage, dpv->finalVoltage, dpv->voltageStep
// , dpv->voltagePulse, dpv->pulseTime, dpv->baseTime);
//     CDC_Transmit_FS(message, strlen((char *)message));
}

void setSWVParameters(SWV *swv, char *parameters, bool channel)
{
    if (!channel)
        swv->channel = Channel1;
    else
        swv->channel = Channel2;

    char *ParametersBuffer;

    ParametersBuffer = strtok(parameters, ";");

    ParametersBuffer = strtok(NULL, ";");
    swv->initialVoltage = atof(ParametersBuffer);

    ParametersBuffer = strtok(NULL, ";");
    swv->finalVoltage = atof(ParametersBuffer);

    ParametersBuffer = strtok(NULL, ";");
    swv->voltageStep = atof(ParametersBuffer)/1000;

    ParametersBuffer = strtok(NULL, ";");
    swv->voltageAmplitude = atof(ParametersBuffer)/1000;

    ParametersBuffer = strtok(NULL, ";");
    swv->frequency = atof(ParametersBuffer);

//     char message[200];
//     snprintf(message, sizeof(message), "%f; %f; %f; %f; %f;", swv->initialVoltage, swv->finalVoltage, swv->voltageStep
// , swv->voltageAmplitude, swv->frequency);
//     CDC_Transmit_FS(message, strlen((char *)message));
}

void setCPParameters(CP *cp, char *parameters, bool channel)
{
    if (!channel)
        cp->channel = Channel1;
    else
        cp->channel = Channel2;

    char *ParametersBuffer;

    ParametersBuffer = strtok(parameters, ";");

    ParametersBuffer = strtok(NULL, ";");
    cp->constCurrent = atof(ParametersBuffer);

    ParametersBuffer = strtok(NULL, ";");
    cp->sampleTime = atof(ParametersBuffer);

    ParametersBuffer = strtok(NULL, ";");
    cp->samplePeriod = atof(ParametersBuffer);

    // char message[200];
    // snprintf(message, sizeof(message), "%f; %f; %f;", cp->constCurrent, cp->sampleTime, cp->samplePeriod);
    // CDC_Transmit_FS(message, strlen((char *)message));
}

void setLSPParameters(LSP *lsp, char *parameters, bool channel)
{
    if (!channel)
        lsp->channel = Channel1;
    else
        lsp->channel = Channel2;

    char *ParametersBuffer;

    ParametersBuffer = strtok(parameters, ";");

    ParametersBuffer = strtok(NULL, ";");
    lsp->initialCurrent = atof(ParametersBuffer)/1000;

    ParametersBuffer = strtok(NULL, ";");
    lsp->finalCurrent = atof(ParametersBuffer)/1000;

    ParametersBuffer = strtok(NULL, ";");
    lsp->currentStep = atof(ParametersBuffer)/1000/1000;

    ParametersBuffer = strtok(NULL, ";");
    lsp->scanRate = atof(ParametersBuffer);

    char message[200];
    //snprintf(message, sizeof(message), "%f; %f; %f; %f;", lsp->initialCurrent, lsp->finalCurrent, lsp->currentStep, lsp->scanRate);
    //CDC_Transmit_FS(message, strlen((char *)message));
}

void setCyPParameters(CyP *cyp, char *parameters, bool channel)
{
    if (!channel)
        cyp->channel = Channel1;
    else
        cyp->channel = Channel2;

    char *ParametersBuffer;

    ParametersBuffer = strtok(parameters, ";");

    ParametersBuffer = strtok(NULL, ";");
    cyp->initialCurrent = atof(ParametersBuffer);

    ParametersBuffer = strtok(NULL, ";");
    cyp->peak1Current = atof(ParametersBuffer);

    ParametersBuffer = strtok(NULL, ";");
    cyp->peak2Current = atof(ParametersBuffer);

    ParametersBuffer = strtok(NULL, ";");
    cyp->finalCurrent = atof(ParametersBuffer);

    ParametersBuffer = strtok(NULL, ";");
    cyp->currentStep = atof(ParametersBuffer)/1000;

    ParametersBuffer = strtok(NULL, ";");
    cyp->scanRate = atof(ParametersBuffer);

    ParametersBuffer = strtok(NULL, ";");
    cyp->cycles = atof(ParametersBuffer);

    char message[200];
    snprintf(message, sizeof(message), "%f; %f; %f; %f; %f; %f; %f;", cyp->initialCurrent, cyp->peak1Current, cyp->peak2Current
    , cyp->finalCurrent, cyp->currentStep, cyp->scanRate, cyp->cycles);
    CDC_Transmit_FS(message, strlen((char *)message));
}
#pragma endregion

#pragma region Build Array
ArrayMeasurementData BuildLSVarray(LSV lsv)
{
    uint32_t totalPoints = (uint32_t)((fabs(lsv.finalVoltage - lsv.initialVoltage) / (lsv.voltageStep)) + 1);
    if (totalPoints > 5000)
        totalPoints = 5000;

    float *arr = (float *)malloc(totalPoints * sizeof(float));

    if (arr == NULL)
    {
        ArrayMeasurementData error = {NULL, 0};
        return error;
    }
    arr[0] = lsv.initialVoltage;
    for (uint32_t i = 1; i <= totalPoints; i++)
    {
        arr[i] = lsv.initialVoltage + i * lsv.voltageStep * ((lsv.finalVoltage > lsv.initialVoltage) ? 1 : -1);
    }

    int stepTime = (lsv.voltageStep * 1000 * 1000) / lsv.scanRate;

    ArrayMeasurementData result = {arr, totalPoints, stepTime, lsv.channel, 1};
    return result;
}

ArrayMeasurementData BuildCVarray(CV cv)
{
    uint32_t points1 = (int)(fabsf((cv.peak1Voltage - cv.initialVoltage) / cv.voltageStep)) + 1;
    uint32_t points2 = (int)(fabsf((cv.peak2Voltage - cv.peak1Voltage) / cv.voltageStep)) + 1;
    uint32_t points3 = (int)(fabsf((cv.finalVoltage - cv.peak2Voltage) / cv.voltageStep)) + 1;

    size_t totalPoints = (points1 + points2 + points3) * cv.cycles;

    if (totalPoints > 5000)
        totalPoints = 5000;

    float *arr = (float *)malloc(totalPoints * sizeof(float));
    if (arr == NULL)
    {
        ArrayMeasurementData error = {NULL, 0};
        return error;
    }

    arr[0] = cv.initialVoltage;
    for (int c = 0; c < cv.cycles; c++)
    {
        uint32_t offset = c * (points1 + points2 + points3);

        for (uint32_t i = 1; i < points1; i++)
        {
            arr[offset + i] = cv.initialVoltage + i * cv.voltageStep *
                                                      ((cv.peak1Voltage > cv.initialVoltage) ? 1 : -1);
        }

        for (uint32_t i = 0; i < points2; i++)
        {
            arr[offset + points1 + i] = cv.peak1Voltage + i * cv.voltageStep *
                                                              ((cv.peak2Voltage > cv.peak1Voltage) ? 1 : -1);
        }

        for (uint32_t i = 0; i < points3; i++)
        {
            arr[offset + points1 + points2 + i] = cv.peak2Voltage + i * cv.voltageStep *
                                                                        ((cv.finalVoltage > cv.peak2Voltage) ? 1 : -1);
        }
    }

    int stepTime = (cv.voltageStep * 1000 * 1000) / cv.scanRate;

    ArrayMeasurementData result = {arr, totalPoints, stepTime, cv.channel, 1};

    return result;
}

ArrayMeasurementData BuildCParray(CP cp)
{
    uint32_t totalPoints = cp.samplePeriod / (cp.sampleTime/1000);
    if (totalPoints == 0)
        totalPoints = 1;
    if (totalPoints > 5000)
        totalPoints = 5000;

    ArrayMeasurementData result;
    result.values = (float *)malloc((totalPoints+1) * sizeof(float));
    result.length = totalPoints;
    result.channel = cp.channel;
    result.timeStep = cp.sampleTime;
    result.potentiostat = 0;

    for (size_t i = 0; i < totalPoints+1; i++)
    {
        result.values[i] = cp.constCurrent;
    }

    return result;
}

ArrayMeasurementData BuildLSParray(LSP lsp)
{
    uint32_t totalPoints = (uint32_t)((fabs(lsp.finalCurrent - lsp.initialCurrent) / lsp.currentStep) + 1);
    if (totalPoints > 5000)
        totalPoints = 5000;

    float *arr = (float *)malloc(totalPoints * sizeof(float));

    if (arr == NULL)
    {
        ArrayMeasurementData error = {NULL, 0};
        return error;
    }
    arr[0] = lsp.initialCurrent;
    for (uint32_t i = 1; i <= totalPoints; i++)
    {
        arr[i] = lsp.initialCurrent + i * lsp.currentStep * ((lsp.finalCurrent > lsp.initialCurrent) ? 1 : -1);
    }

    int stepTime = (lsp.currentStep * 1000 * 1000) / lsp.scanRate;

    ArrayMeasurementData result = {arr, totalPoints, stepTime, lsp.channel, 0};

    return result;
}

ArrayMeasurementData BuildCyParray(CyP cyp)
{
    uint32_t points1 = (int)(fabsf((cyp.peak1Current - cyp.initialCurrent) / cyp.currentStep)) + 1;
    uint32_t points2 = (int)(fabsf((cyp.peak2Current - cyp.peak1Current) / cyp.currentStep)) + 1;
    uint32_t points3 = (int)(fabsf((cyp.finalCurrent - cyp.peak2Current) / cyp.currentStep)) + 1;

    size_t totalPoints = (points1 + points2 + points3) * cyp.cycles;

    if (totalPoints > 5000)
        totalPoints = 5000;

    float *arr = (float *)malloc(totalPoints * sizeof(float));
    if (arr == NULL)
    {
        ArrayMeasurementData error = {NULL, 0};
        return error;
    }
    arr[0] = cyp.initialCurrent;
    for (int c = 0; c < cyp.cycles; c++)
    {
        uint32_t offset = c * (points1 + points2 + points3);

        for (uint32_t i = 1; i < points1; i++)
        {
            arr[offset + i] = cyp.initialCurrent + i * cyp.currentStep *
                                                       ((cyp.peak1Current > cyp.initialCurrent) ? 1 : -1);
        }

        for (uint32_t i = 0; i < points2; i++)
        {
            arr[offset + points1 + i] = cyp.peak1Current + i * cyp.currentStep *
                                                               ((cyp.peak2Current > cyp.peak1Current) ? 1 : -1);
        }

        for (uint32_t i = 0; i < points3; i++)
        {
            arr[offset + points1 + points2 + i] = cyp.peak2Current + i * cyp.currentStep *
                                                                         ((cyp.finalCurrent > cyp.peak2Current) ? 1 : -1);
        }
    }

    int stepTime = (cyp.currentStep * 1000 * 1000) / cyp.scanRate;

    ArrayMeasurementData result = {arr, totalPoints, stepTime, cyp.channel, 0};

    return result;
}

#pragma endregion

void SMU_ProcessComandIT() {}
void SMU_AbortIT() {}

void SMU_UpdateInterfacePC() {}
void SMU_UpdateInterfaceDisplay() {}

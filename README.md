# USCIS Case Batch Query

Query USCIS case status by receipt number in a batch. 

## USCIS Receipt Numbers Explained
USCIS receipt numbers start with three letters followed by a series of numbers, in the form of WAC-20-123-45678. Here is the break down of the letters and numbers

### First three letters - Processing Service Center 
***WAC***-20-123-45678

The first three letters indicate the USCIS service center which is processing the petition. They include:
- CSC – California Service Center
- EAC – Eastern Adjudication Center (now known as Vermont Service Center)
- IOE – ELIS (efile)
- LIN – Lincoln Service Center (now known as Nebraska Service Center)
- MSC – Missouri Service Center (now known as National Benefits Center)
- NBC – National Benefits Center
- NSC – Nebraska Service Center
- SRC – Southern Regional Center (now known as Texas Service Center)
- TSC – Texas Service Center
- VSC – Vermont Service Center
- WAC – Western Adjudication Center (now known as California Service Center)

### Second set of two digits - Fiscal Year
WAC-***20***-123-45678

The next two digits represent the fiscal year of the case received. A fiscal year begins on Oct 1 and ends on Sep 30. In this example the case was received in 2020 fiscal year.

### Third set of three digits - Computer Workday
WAC-20-***123**-45678

These three digits represent the computer workday on which the receipt was processed and the fee was taken. A computer workday is essentially the same thing as a regular workday, which excludes weekends and most holidays. In this example the case was processed on the 123rd workday of 2020 fiscal year.

### Last set of five digits - Case Number
WAC-20-123-***45678***
Finally, the last five digits are used to identify uniquely the petition filed. It has been observed that these are sequential numbers which are issued as cases are being processed at the intake facility. Cases filed together are often given sequential (or close to sequential) numbers for the last five digits (and overall).

## What you can do with this tool
Using this tool you can query case status by receipt number that are close to yours to see how other cases of the same type is going. Also you can research on case history to estimate the processing time of the type of your interest. This is all because USCIS's unwillingness of releasing any information with regard to their procedure and exact processing time.

Good luck with your petition!

## *Warning*
### Making too many queries in a short period of time may be considered as a DoS attack. Your IP may be blocked and you may face legal issues. Make sure you only query for a reasonable number of cases!

## Configurations
You can configure the following settings in app.config
### DefaultReceiptNumber
Default receipt number will be loaded at start up
### DefaultNextCases
Default next cases will be loaded at start up
### MaxNextCases
Default limit for next cases. Strongly recommend to set it under 100

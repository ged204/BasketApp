Feature: Basket
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

@mytag
Scenario: Apply a single gift voucher to a basket of two items
	Given I have added the following items to my basket
	| Name   | Price | Category |
	| Hat    | 10.50 | Clothes  |
	| Jumper | 54.65 | Clothes  |
	When I apply the following gift vouchers
	| VoucherCode | Discount |
	| XXX-XXX     | 5.00     |
	Then The total price should be 60.15
	And There should be no error message
	
Scenario: Apply a single offer voucher to a basket that meets the threshold
	Given I have added the following items to my basket
	| Name   | Price | Category |
	| Hat    | 25.00 | Clothes  |
	| Jumper | 26.00 | Clothes  |
	When I apply the following offer vouchers
	| VoucherCode | Discount | Threshold |
	| YYY-YYY     | 5.00     | 50.00     |
	Then The total price should be 46.00
	And There should be no error message

Scenario: Apply a single gift voucher and offer voucher to a basket that meets the discount threshold
	Given I have added the following items to my basket
	| Name   | Price | Category |
	| Hat    | 25.00 | Clothes  |
	| Jumper | 26.00 | Clothes  |
	When I apply the following gift vouchers
	| VoucherCode | Discount |
	| XXX-XXX     | 5.00     |
	And I apply the following offer vouchers
	| VoucherCode | Discount | Threshold |
	| YYY-YYY     | 5.00     | 50.00     |
	Then The total price should be 41.00
	And There should be no error message

Scenario: Apply a single category offer voucher to a basket with qualifying items that meets the threshold
	Given I have added the following items to my basket
	| Name       | Price | Category  |
	| Hat        | 25.00 | Clothes   |
	| Jumper     | 26.00 | Clothes   |
	| Head Light | 3.50  | Head Gear |
	When I apply the following category offer vouchers
	| VoucherCode | Discount | ProductCategory | Threshold |
	| YYY-YYY     | 5.00     | Head Gear       | 50.00     |
	Then The total price should be 51.00
	And There should be no error message

Scenario: Apply a single category offer voucher to a basket with no qualifying items
	Given I have added the following items to my basket
	| Name   | Price | Category |
	| Hat    | 25.00 | Clothes  |
	| Jumper | 26.00 | Clothes  |
	When I apply the following category offer vouchers
	| VoucherCode | Discount | ProductCategory  |
	| YYY-YYY     | 5.00     | Head Gear        |
	Then The total price should be 51.00
	And The error message should be "There are no products in your basket applicable to voucher YYY-YYY."

Scenario: Apply a single category offer voucher to a basket with qualifying items that doesn't meet the threshold
	Given I have added the following items to my basket
	| Name       | Price | Category  |
	| Hat        | 25.00 | Clothes   |
	| Jumper     | 26.00 | Clothes   |
	| Head Light | 3.50  | Head Gear |
	When I apply the following category offer vouchers
	| VoucherCode | Discount | ProductCategory | Threshold |
	| YYY-YYY     | 5.00     | Head Gear       | 70.00     |
	Then The total price should be 54.50
	And The error message should be "You have not reached the spend threshold for the voucher YYY-YYY. Spend another £15.51 to receive the £5.00 discount from your basket total."

Scenario: Apply an offer voucher to a basket with a gift voucher item
	Given I have added the following items to my basket
	| Name         | Price | Category      |
	| Hat          | 25.00 | Clothes       |
	| Gift Voucher | 30.00 | Gift Vouchers |
	When I apply the following offer vouchers
	| VoucherCode | Discount | Threshold |
	| YYY-YYY     | 5.00     | 50.00     |
	Then The total price should be 55.00
	And The error message should be "You have not reached the spend threshold for the voucher YYY-YYY. Spend another £25.01 to receive the £5.00 discount from your basket total."

Scenario: Apply two valid offer vouchers to the same basket
	Given I have added the following items to my basket
	| Name       | Price | Category  |
	| Hat        | 25.00 | Clothes   |
	| Jumper     | 26.00 | Clothes   |
	When I apply the following offer vouchers
	| VoucherCode | Discount | Threshold |
	| YYY-YYY     | 5.00     | 50.00     |
	| ZZZ-ZZZ     | 5.00     | 50.00     |
	Then The total price should be 46.00
	And The error message should be "You can only use one offer voucher at a time."

Scenario: Apply a single offer voucher to a basket that meets the threshold and then remove an item so the basket doesn't meet the threshold
	Given I have added the following items to my basket
	| Name       | Price | Category  |
	| Hat        | 25.00 | Clothes   |
	| Jumper     | 26.00 | Clothes   |
	When I apply the following offer vouchers
	| VoucherCode | Discount | Threshold |
	| YYY-YYY     | 5.00     | 50.00     |
	And I then remove the following item from the basket
	| Name       | Price | Category  |
	| Hat        | 25.00 | Clothes   |
	Then The total price should be 26.00
	And The error message should be "You have not reached the spend threshold for the voucher YYY-YYY. Spend another £24.01 to receive the £5.00 discount from your basket total."


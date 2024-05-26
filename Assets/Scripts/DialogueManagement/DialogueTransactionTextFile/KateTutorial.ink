EXTERNAL buyingandsellingApples(AppleActivity)
Hello there im kate, Ill teach you the basic market Interactions #speaker:Apple kate #image:AppleKate
First lets initiate our transaction using the options display
    *[Yes please]
        yes please #speaker:Eustace #image:PlayerImage
        Press 'B' to buy and 'V' to sell apples to me#speaker:Apple kate #image:AppleKate
        ~buyingandsellingApples("BuyandSellApples")
        have fun!!!!
    *[Nah thank you] 
        No, thank you #speaker:Eustace #image:PlayerImage
        We still need to initiate our transaction, sorry bud #speaker:Apple kate #image:AppleKate
        Press 'B' to buy and 'V' to sell apples to me
        ~buyingandsellingApples("BuyandSellApples")
        have fun!!!!
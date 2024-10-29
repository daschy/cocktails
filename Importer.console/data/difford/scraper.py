import requests
from bs4 import BeautifulSoup


def scrape_cocktail_recipe(url):
    headers = {
        "User-Agent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.3"
    }
    # Send a request to the URL
    response = requests.get(url, headers=headers)

    # Check if the request was successful
    if response.status_code == 200:
        # Parse the content with BeautifulSoup
        soup = BeautifulSoup(response.text, "html.parser")

        # Scrape the image
        image = soup.find("img")  # Adjust selector based on actual site for images
        image_url = image["src"] if image else "No image found"

        # Scrape garnishes
        garnish = soup.select_one(
            "#sticky-anchor>div>div>div.cell.auto.divide-right-large>div>article>div>div:nth-child(3)>p"
        )  # Adjust this based on actual HTML
        garnish_text = (
            garnish.get_text(strip=True) if garnish else "No garnish information"
        )

        # Scrape glass type
        glass = soup.find(
            "a", attrs={"href": "/encyclopedia/329/cocktails/cocktail-glassware"}
        )  # Adjust this based on actual HTML
        glass_text = glass.get_text(strip=True) if glass else "No glass type provided"

        # Scrape how to make
        how_to_make = soup.select_one(
            "#sticky-anchor > div > div > div.cell.auto.divide-right-large > div > article > div > div:nth-child(4) > p"
        )  # Adjust this based on actual HTML
        how_to_make_text = (
            how_to_make.get_text(strip=True) if how_to_make else "No instructions found"
        )

        ingredients_list = []
        table = soup.find("table", class_="ingredients-table")

        for row in table.find_all("tr"):
            cells = row.find_all("td")
            if len(cells) == 2:
                quantity = cells[0].get_text(strip=True)
                name = cells[1].get_text(strip=True)
                ingredients_list.append((quantity, name))

        # Scrape history
        history = soup.select_one(
            "#sticky-anchor > div > div > div.cell.auto.divide-right-large > div > article > div > div:nth-child(14) > p:nth-child(2)"
        )  # Adjust this for actual history
        history_text = (
            history.get_text(strip=False).strip()
            if history
            else "No history information"
        )

        review = soup.select_one(
            "#sticky-anchor > div > div > div.cell.auto.divide-right-large > div > article > div > div:nth-child(12) > p"
        )
        review_text = (
            review.get_text(strip=False).strip() if review else "No review information"
        )
        # Output the results
        print("Image URL:", image_url)
        print("Garnish:", garnish_text)
        print("Glass Type:", glass_text)
        print("How to Make:", how_to_make_text)
        print("Ingredients:")
        for ingredient in ingredients_list:
            print(f"  - Quantity: {ingredient[0]}, Name: {ingredient[1]}")
        print("History:", history_text)
        print("Review:", review_text)

    else:
        print(f"Failed to retrieve page. Status code: {response.status_code}")


# Example usage
if __name__ == "__main__":
    url = "https://www.diffordsguide.com/cocktails/recipe/2671/gloom-chaser-embury"  # Replace with the actual cocktail recipe URL
    scrape_cocktail_recipe(url)

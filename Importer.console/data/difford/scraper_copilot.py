import requests
from bs4 import BeautifulSoup

url = "https://www.diffordsguide.com/cocktails/recipe/2671/gloom-chaser-embury"
response = requests.get(url)
soup = BeautifulSoup(response.content, 'html.parser')

# Extracting cocktail name
cocktail_name = soup.find('h1', class_='recipe-title').text.strip()

# Extracting cocktail ingredients name and quantity
ingredients = []
ingredient_rows = soup.find_all('tr', class_='ingredients-row')
for row in ingredient_rows:
    quantity = row.find('td', class_='ingredients-quantity').text.strip()
    ingredient = row.find('td', class_='ingredients-description').text.strip()
    ingredients.append((quantity, ingredient))

# Extracting how to make
how_to_make = soup.find('div', class_='recipe-method').text.strip()

# Extracting review
review = soup.find('div', class_='recipe-review').text.strip()

# Extracting history
history = soup.find('div', class_='recipe-history').text.strip()

# Extracting image
image_url = soup.find('img', class_='recipe-image')['src']

# Extracting glass to use
glass = soup.find('div', class_='recipe-glass').text.strip()

# Printing the extracted information
print(f"Cocktail Name: {cocktail_name}")
print(f"Cocktail Ingredients: {ingredients}")
print(f"How to Make: {how_to_make}")
print(f"Review: {review}")
print(f"History: {history}")
print(f"Image URL: {image_url}")
print(f"Glass to Use: {glass}")
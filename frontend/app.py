from flask import Flask, request, Response  
import matplotlib.pyplot as plt  
import numpy as np  
import io  
  
app = Flask(__name__)  
  
data = [2, 4, 1, 5, 6, 7]  # initial data array  
  
@app.route('/')  
def bar_chart():  
    plt.bar(np.arange(len(data)), data)  
    plt.title('Bar Chart')  
    plt.xlabel('Index')  
    plt.ylabel('Value')  
    img = io.BytesIO()  
    plt.savefig(img, format='png')  
    plt.close()  
    img.seek(0)  
    return Response(response=img, content_type='image/png')  
  
@app.route('/update', methods=['POST'])  
def update_data():  
    global data  
    new_data = request.get_json()  
    data = new_data  
    return 'Data updated successfully!'  
  
if __name__ == '__main__':  
    app.run(debug=True)  

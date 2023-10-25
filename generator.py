# Sample data with tab-separated words and paragraphs
text_data = """This\tis\tthe\tfirst\tline\tof\ttext.\n\nHere\tis\tthe\tsecond\tline\tin\ta\tnew\tparagraph.\n\nAnd\tthis\tis\tthe\tthird\tline\tin\tthe\tsame\tparagraph.\n\nLet's\tstart\ta\tnew\tparagraph\twith\tmore\ttab-separated\twords.\n\nWord1\tWord2\tWord3\tWord4\tWord5\n"""

# Save the data to a .txt file
with open("sample_text_file.txt", "w") as file:
    file.write(text_data)

print("File 'sample_text_file.txt' has been created.")